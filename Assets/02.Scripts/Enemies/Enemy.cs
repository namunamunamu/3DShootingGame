using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{   
    [SerializeField]
    private EnemyDataSO enemyData;

    // 2. 현재 상태를 지정
    public EEnemyState CurrentState = EEnemyState.Idle;


    public List<Vector3> PatrolPoints;

    private int _nextPointIndex;

    private float _attackTimer;
    private float _idleTimer;

    private GameObject _player;
    private NavMeshAgent _agent;
    private Vector3 _initialPosition;
    private Rigidbody _rigidBody;
    private const float MINDISTANCE = 0.2f;

    [SerializeField]
    private EnemyStateMachine enemyStateMachine;

    private void Awake()
    {
        _initialPosition = transform.position;
        InitializePartolPoint();
    }


    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = enemyData.MoveSpeed;

        _player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        switch(CurrentState)
        {
            case EEnemyState.Idle:
            {
                // Idle();
                break;
            }

            case EEnemyState.Patrol:
            {
                // Patrol();
                break;
            }

            case EEnemyState.Trace:
            {
                // Trace();
                break;
            }

            case EEnemyState.Attack:
            {
                // Attack();
                break;
            }

            case EEnemyState.Return:
            {
                // Return();
                break;
            }

            default:
                break;
        }
    }

    private void InitializePartolPoint()
    {
        PatrolPoints.Clear();
        PatrolPoints.Add(_initialPosition);
        for(int i=1; i<enemyData.PatrolPointCount; i++)
        {
            PatrolPoints.Add(new Vector3(Random.Range(_initialPosition.x - enemyData.PatrolDistance, _initialPosition.x + enemyData.PatrolDistance),
                                         1,
                                         Random.Range(_initialPosition.z - enemyData.PatrolDistance, _initialPosition.z + enemyData.PatrolDistance)));
        }
    }

    public void TakeDamage(Damage damage)
    {
        if(CurrentState == EEnemyState.Die)
        {
            return;
        }

        Vector3 knockBackDirecton = (transform.position - damage.From.transform.position).normalized;
        _rigidBody.AddForce(knockBackDirecton * damage.KnockBackPower);

        enemyData.Health -= damage.Value;
        Debug.Log($"Health: {enemyData.Health}");

        if(enemyData.Health <= 0)
        {
            CurrentState = EEnemyState.Die;
            Debug.Log($"상태전환: {CurrentState} -> Die");
            StartCoroutine(Die_Coroutine());
            return;
        }

        if(CurrentState != EEnemyState.Damaged)
        {
            Debug.Log("상태 전환: Any->Damaged");
            CurrentState = EEnemyState.Damaged;
            StartCoroutine(Damaged_Coroutine(damage));    
        }
    }

            // 3. 상태 함수들을 구현
    private void Idle()
    {
        _idleTimer += Time.deltaTime;
        if(_idleTimer >= enemyData.IdleTime)
        {
            Debug.Log("상태전환: Idle -> Patrol");
            CurrentState = EEnemyState.Patrol;
            _idleTimer = 0f;
            return;
        }
        
        if(Vector3.Distance(transform.position, _player.transform.position) < enemyData.FindDistance)
        {
            Debug.Log("상태 전환: Idle->Trace");
            CurrentState = EEnemyState.Trace;
        }
    }

    private void Trace()
    {
        if(Vector3.Distance(transform.position, _player.transform.position) >= enemyData.ReturnDistance)
        {
            Debug.Log("상태전환: Trace -> Return");
            CurrentState = EEnemyState.Return;
            return;
        }

        if(Vector3.Distance(transform.position, _player.transform.position) <= enemyData.AttackDistance)
        {
            Debug.Log("상태전환: Trace -> Attack");
            CurrentState = EEnemyState.Attack;
            return;
        }
        _agent.SetDestination(_player.transform.position);
    }

    private void Patrol()
    {
        if(Vector3.Distance(transform.position, _player.transform.position) < enemyData.FindDistance)
        {
            Debug.Log("상태 전환: Patrol->Trace");
            CurrentState = EEnemyState.Trace;
            return;
        }
        _agent.SetDestination(PatrolPoints[_nextPointIndex]);

        float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(PatrolPoints[_nextPointIndex].x, PatrolPoints[_nextPointIndex].z));
        if(distance <= MINDISTANCE)
        {
            Debug.Log("상태 전환: Patrol->Idle");
            CurrentState = EEnemyState.Idle;

            _nextPointIndex++;
            if(_nextPointIndex >= enemyData.PatrolPointCount)
            {
                InitializePartolPoint();
                _nextPointIndex = 0;
            }
        }
    }

    private void Return()
    {
        if(Vector3.Distance(transform.position, _player.transform.position) < enemyData.FindDistance)
        {
            Debug.Log("상태 전환: return->Trace");
            CurrentState = EEnemyState.Trace;
            return;
        }

        float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), 
                                          new Vector2(PatrolPoints[_nextPointIndex].x, PatrolPoints[_nextPointIndex].z));
        if(distance <= MINDISTANCE)
        {
            Debug.Log("상태전환: Return -> Idle");
            transform.position = PatrolPoints[_nextPointIndex];
            CurrentState = EEnemyState.Idle;
            return;
        }
        _agent.SetDestination(PatrolPoints[_nextPointIndex]);
    }

    private void Attack()
    {
        if(Vector3.Distance(transform.position, _player.transform.position) > enemyData.AttackDistance)
        {
            Debug.Log("상태전환: Attack -> Trace");
            CurrentState = EEnemyState.Trace;
            _attackTimer = 0f;
            return;
        }

        _attackTimer += Time.deltaTime;
        if(_attackTimer >= enemyData.AttackCoolTime)
        {
            Debug.Log("공격!");
            _attackTimer = 0f;
        }
    }

    private IEnumerator Damaged_Coroutine(Damage damage)
    {
        _agent.ResetPath();

        yield return new WaitForSeconds(enemyData.DamageTime);
        Debug.Log("상태전환: Damaged -> Trace");
        CurrentState = EEnemyState.Trace;
    }

    private IEnumerator Die_Coroutine()
    {
        yield return new WaitForSeconds(enemyData.DeathTime);
        gameObject.SetActive(false);
    }


}
