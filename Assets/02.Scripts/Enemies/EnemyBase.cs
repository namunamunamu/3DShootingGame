using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour
{
    public EnemyDataSO EnemyData;
    public List<IEnemeyState> EnemyStates;
    public Dictionary<EEnemyState, IEnemeyState> EnemyStateDict;

    public NavMeshAgent Agent => _agent;
    public Vector3 InitialPosition => _initialPosition;
    public Vector3 ReturnPoint {get; set;}

    private EnemyStateMachine _enemyFSM;
    private GameObject _player;
    private NavMeshAgent _agent;
    private Rigidbody _rigidBody;
    private Vector3 _initialPosition;

    public float MINDISTANCE = 0.2f;


    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = EnemyData.MoveSpeed;
        _initialPosition = gameObject.transform.position;
        ReturnPoint = _initialPosition;

        SetStateDictionary();

    }

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _enemyFSM = new EnemyStateMachine(this, EnemyStateDict);
        _enemyFSM.SetPlayer(_player);
        _enemyFSM.SetState(EEnemyState.Idle);
    }
    private void Update()
    {
        _enemyFSM.ExcuteState();
    }

    protected virtual void SetStateDictionary()
    {
        EnemyStateDict = new Dictionary<EEnemyState, IEnemeyState>();
    }

    public void TakeDamage(Damage damage)
    {
        if(_enemyFSM.CurrentStateType == EEnemyState.Die)
        {
            return;
        }

        Vector3 knockBackDirecton = (transform.position - damage.From.transform.position).normalized;
        _rigidBody.AddForce(knockBackDirecton * damage.KnockBackPower);

        EnemyData.Health -= damage.Value;
        Debug.Log($"Health: {EnemyData.Health}");

        if(EnemyData.Health <= 0)
        {
            _enemyFSM.SetState(EEnemyState.Die);
            Debug.Log($"상태전환: {_enemyFSM.CurrentStateType} -> Die");
            StartCoroutine(Die_Coroutine());
            return;
        }

        if(_enemyFSM.CurrentStateType != EEnemyState.Damaged)
        {
            Debug.Log("상태 전환: Any->Damaged");
            _enemyFSM.SetState(EEnemyState.Damaged);
            StartCoroutine(Damaged_Coroutine(damage));    
        }
    }

    private IEnumerator Damaged_Coroutine(Damage damage)
    {
        _agent.ResetPath();

        yield return new WaitForSeconds(EnemyData.DamageTime);
        Debug.Log("상태전환: Damaged -> Trace");
        _enemyFSM.SetState(EEnemyState.Trace);
    }

    private IEnumerator Die_Coroutine()
    {
        yield return new WaitForSeconds(EnemyData.DeathTime);
        gameObject.SetActive(false);
    }
}
