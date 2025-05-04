using UnityEngine;

public class EnemyReturnState : IEnemeyState
{
    private  EnemyBase _enemy;
    private GameObject _player; 
    private EnemyStateMachine _stateMachine;

    public void InitializeState(EnemyStateMachine stateMachine, EnemyBase enemy, GameObject player)
    {
        _enemy = enemy;
        _player = player;
        _stateMachine = stateMachine;
    }

    public void ExcuteState()
    {
        if(Vector3.Distance(_enemy.transform.position, _player.transform.position) < _enemy.EnemyData.FindDistance)
        {
            Debug.Log("상태 전환: return->Trace");
            _stateMachine.SetState(EEnemyState.Trace);
            return;
        }

        float distance = Vector2.Distance(new Vector2(_enemy.transform.position.x, _enemy.transform.position.z), 
                                          new Vector2(_enemy.ReturnPoint.x, _enemy.ReturnPoint.z));
        if(distance <= _enemy.MINDISTANCE)
        {
            Debug.Log("상태전환: Return -> Idle");
            _enemy.transform.position = _enemy.ReturnPoint;
            _stateMachine.SetState(EEnemyState.Idle);
            return;
        }

        _enemy.Agent.SetDestination(_enemy.ReturnPoint);
    }

    public void EndState()
    {

    }
}
