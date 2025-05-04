using UnityEngine;

public class EnemyIdleState : IEnemeyState
{
    private  EnemyBase _enemy;
    private GameObject _player; 
    private EnemyStateMachine _stateMachine;

    private float _idleTimer;

    public void InitializeState(EnemyStateMachine stateMachine, EnemyBase enemy, GameObject player)
    {
        _enemy = enemy;
        _player = player;
        _stateMachine = stateMachine;
    }

    public void ExcuteState()
    {
        _idleTimer += Time.deltaTime;
        if(_idleTimer >= _enemy.EnemyData.IdleTime)
        {
            Debug.Log("상태전환: Idle -> Patrol");
            _stateMachine.SetState(EEnemyState.Patrol);
            _idleTimer = 0f;
            return;
        }

        if(Vector3.Distance(_enemy.transform.position, _player.transform.position) < _enemy.EnemyData.FindDistance)
        {
            Debug.Log("상태 전환: Idle->Trace");
            _stateMachine.SetState(EEnemyState.Trace);
        }
    }

    public void EndState()
    {
        _idleTimer = 0f;
    }
}
