using UnityEngine;

public class EnemyTraceState : IEnemeyState
{
    private EnemyBase _enemy;
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
        if(Vector3.Distance(_enemy.transform.position, _player.transform.position) >= _enemy.EnemyData.ReturnDistance)
        {
            Debug.Log("상태전환: Trace -> Return");
            _stateMachine.SetState(EEnemyState.Return);
            return;
        }

        if(Vector3.Distance(_enemy.transform.position, _player.transform.position) <= _enemy.EnemyData.AttackDistance)
        {
            Debug.Log("상태전환: Trace -> Attack");
            _stateMachine.SetState(EEnemyState.Attack);
            return;
        }
        _enemy.Agent.SetDestination(_player.transform.position);
    }

    public void EndState()
    {

    }
}
