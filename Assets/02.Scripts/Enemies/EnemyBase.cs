using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EEnemyType
{
    Basic,
    Chase,
    Count
}

public abstract class EnemyBase : MonoBehaviour
{
    public EnemyDataSO EnemyData;
    public Dictionary<EEnemyState, IEnemeyState> EnemyStateDict;

    public NavMeshAgent Agent => _agent;
    public Vector3 InitialPosition => _initialPosition;
    public Vector3 ReturnPoint {get; set;}

    protected EnemyStateMachine _enemyFSM;
    protected GameObject _player;
    private NavMeshAgent _agent;
    private Vector3 _initialPosition;

    private int _enemyHealth;

    public float MINDISTANCE = 0.2f;


    protected virtual void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = EnemyData.MoveSpeed;
        _initialPosition = gameObject.transform.position;
        ReturnPoint = _initialPosition;

        _enemyHealth = EnemyData.Health;

        SetStateDictionary();
        _enemyFSM = new EnemyStateMachine(this, EnemyStateDict);
    }

    protected virtual void Update()
    {
        _enemyFSM.ExcuteState();
    }

    public void Initialize(GameObject player)
    {
        _enemyHealth = EnemyData.Health;
        _enemyFSM.SetPlayer(player);
        _enemyFSM.SetState(EEnemyState.Idle);
    }

    protected void SetStateDictionary()
    {
        EnemyStateDict = new Dictionary<EEnemyState, IEnemeyState>();
        EnemyStateDict.Add(EEnemyState.Idle, new EnemyIdleState());
        EnemyStateDict.Add(EEnemyState.Trace, new EnemyTraceState());
        EnemyStateDict.Add(EEnemyState.Return, new EnemyReturnState());
        EnemyStateDict.Add(EEnemyState.Attack, new EnemyAttackState());
        EnemyStateDict.Add(EEnemyState.Damaged, new EnemyDamagedState());
        EnemyStateDict.Add(EEnemyState.Patrol, new EnemyPatrolState());
        EnemyStateDict.Add(EEnemyState.Die, new EnemyDieState());
    }

    public void TakeDamage(Damage damage)
    {
        if(_enemyFSM.CurrentStateType == EEnemyState.Die)
        {
            return;
        }

        Agent.ResetPath();
        Vector3 knockBackDirecton = (transform.position - damage.From.transform.position).normalized;
        transform.position += new Vector3(knockBackDirecton.x, 0, knockBackDirecton.z) * damage.KnockBackPower;

        _enemyHealth -= damage.Value;
        Debug.Log($"Health: {_enemyHealth}");

        if(_enemyHealth <= 0)
        {
            _enemyFSM.SetState(EEnemyState.Die);
            Debug.Log($"상태전환: {_enemyFSM.CurrentStateType} -> Die");
            return;
        }

        Debug.Log($"상태 전환: {_enemyFSM.CurrentStateType} -> Damaged");
        _enemyFSM.SetState(EEnemyState.Damaged);
    }
}
