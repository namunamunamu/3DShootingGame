using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    // 2. 현재 상태를 지정
    public EEnemyState CurrentState = EEnemyState.Idle;

    public int Health           = 100;
    public float FindDistance   = 7f;
    public float ReturnDistance = 10f;
    public float AttackDistance = 2.5f;
    public float MoveSpeed      = 3.3f;
    public float AttackCoolTime = 2f;
    public float DamageTime     = 0.5f;
    public float DeathTime      = 2f;
    public float IdleTime       = 3f;
    public float PatrolDistance = 10f;
    public int PatrolPointCount = 3;
    public List<Vector3> PatrolPoints;

    private int _nextPointIndex;

    private float _attackTimer;
    private float _idleTimer;

    private GameObject _player;
    private CharacterController _characterController;
    private Vector3 _initialPosition;
    private Vector3 _gravity;
    private const float Gravity = -9.8f;

    private void Awake()
    {
        _initialPosition = transform.position;
        InitializePartolPoint();
        _gravity = new Vector3(0, Gravity, 0);
    }


    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        _characterController.Move(_gravity * Time.deltaTime);

        switch(CurrentState)
        {
            case EEnemyState.Idle:
            {
                Idle();
                break;
            }

            case EEnemyState.Patrol:
            {
                Patrol();
                break;
            }

            case EEnemyState.Trace:
            {
                Trace();
                break;
            }

            case EEnemyState.Attack:
            {
                Attack();
                break;
            }

            case EEnemyState.Return:
            {
                Return();
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
        for(int i=1; i<PatrolPointCount; i++)
        {
            PatrolPoints.Add(new Vector3(Random.Range(_initialPosition.x - PatrolDistance, _initialPosition.x + PatrolDistance),
                                         1,
                                         Random.Range(_initialPosition.z - PatrolDistance, _initialPosition.z + PatrolDistance)));
        }
    }

    public void TakeDamage(Damage damage)
    {
        if(CurrentState == EEnemyState.Damaged || CurrentState == EEnemyState.Die)
        {
            return;
        }

        Health -= damage.Value;
        Debug.Log($"Health: {Health}");
        Vector3 knockBackDirecton = (transform.position - damage.From.transform.position).normalized;
        _characterController.Move(knockBackDirecton * damage.KnockBackPower);

        if(Health <= 0)
        {
            CurrentState = EEnemyState.Die;
            Debug.Log($"상태전환: {CurrentState} -> Die");
            StartCoroutine(Die_Coroutine());
            return;
        }

        Debug.Log("상태 전환: Any->Damaged");
        CurrentState = EEnemyState.Damaged;
        StartCoroutine(Damaged_Coroutine());
    }

    // 3. 상태 함수들을 구현
    private void Idle()
    {
        _idleTimer += Time.deltaTime;
        if(_idleTimer >= IdleTime)
        {
            Debug.Log("상태전환: Idle -> Patrol");
            CurrentState = EEnemyState.Patrol;
            _idleTimer = 0f;
            return;
        }
        
        if(Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태 전환: Idle->Trace");
            CurrentState = EEnemyState.Trace;
        }
    }

    private void Trace()
    {
        if(Vector3.Distance(transform.position, _player.transform.position) >= ReturnDistance)
        {
            Debug.Log("상태전환: Trace -> Return");
            CurrentState = EEnemyState.Return;
            return;
        }

        if(Vector3.Distance(transform.position, _player.transform.position) <= AttackDistance)
        {
            Debug.Log("상태전환: Trace -> Attack");
            CurrentState = EEnemyState.Attack;
            return;
        }

        Vector3 dir = (_player.transform.position - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Patrol()
    {
        if(Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태 전환: Patrol->Trace");
            CurrentState = EEnemyState.Trace;
            return;
        }

        Vector3 dir = (PatrolPoints[_nextPointIndex] - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, PatrolPoints[_nextPointIndex]) <= _characterController.minMoveDistance*100)
        {
            Debug.Log("상태 전환: Patrol->Idle");
            CurrentState = EEnemyState.Idle;

            _nextPointIndex++;
            if(_nextPointIndex >= PatrolPointCount)
            {
                InitializePartolPoint();
                _nextPointIndex = 0;
            }
        }
    }

    private void Return()
    {
        if(Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태 전환: return->Trace");
            CurrentState = EEnemyState.Trace;
            return;
        }

        if(Vector3.Distance(transform.position, PatrolPoints[_nextPointIndex]) <= _characterController.minMoveDistance*100)
        {
            Debug.Log("상태전환: Return -> Idle");
            transform.position = PatrolPoints[_nextPointIndex];
            CurrentState = EEnemyState.Idle;
            return;
        }

        Vector3 dir = (PatrolPoints[_nextPointIndex] - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        if(Vector3.Distance(transform.position, _player.transform.position) > AttackDistance)
        {
            Debug.Log("상태전환: Attack -> Trace");
            CurrentState = EEnemyState.Trace;
            _attackTimer = 0f;
            return;
        }

        _attackTimer += Time.deltaTime;
        if(_attackTimer >= AttackCoolTime)
        {
            Debug.Log("공격!");
            _attackTimer = 0f;
        }
    }

    private IEnumerator Damaged_Coroutine()
    {
        yield return new WaitForSeconds(DamageTime);
        Debug.Log("상태전환: Damaged -> Trace");
        CurrentState = EEnemyState.Trace;
    }

    private IEnumerator Die_Coroutine()
    {
        yield return new WaitForSeconds(DeathTime);
        gameObject.SetActive(false);
    }
}
