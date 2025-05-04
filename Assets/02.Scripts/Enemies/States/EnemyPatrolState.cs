using UnityEngine;

public class EnemyPatrolState : IEnemeyState
{
    private  Enemy _enemy;
    private GameObject _player; 
    private EnemyStateMachine _stateMachine;

    public void InitializeState(EnemyStateMachine stateMachine, EnemyBase enemy, GameObject player)
    {
        _enemy = (Enemy)enemy;
        _player = player;
        _stateMachine = stateMachine;
    }

    public void ExcuteState()
    {
        if(Vector3.Distance(_enemy.transform.position, _player.transform.position) < _enemy.EnemyData.FindDistance)
        {
            Debug.Log("상태 전환: Patrol->Trace");
            _stateMachine.SetState(EEnemyState.Trace);
            return;
        }

        _enemy.Agent.SetDestination(_enemy.PatrolPoints[_enemy.NextPointIndex]);
        _enemy.ReturnPoint = _enemy.PatrolPoints[_enemy.NextPointIndex];

        float distance = Vector2.Distance(new Vector2(_enemy.transform.position.x, _enemy.transform.position.z), new Vector2(_enemy.PatrolPoints[_enemy.NextPointIndex].x, _enemy.PatrolPoints[_enemy.NextPointIndex].z));
        if(distance <= _enemy.MINDISTANCE)
        {
            Debug.Log("상태 전환: Patrol->Idle");
            _stateMachine.SetState(EEnemyState.Idle);

            _enemy.NextPointIndex++;
            if(_enemy.NextPointIndex >= _enemy.EnemyData.PatrolPointCount)
            {
                _enemy.InitializePartolPoint();
                _enemy.NextPointIndex = 0;
            }
        }
    }

    public void EndState()
    {

    }
}
