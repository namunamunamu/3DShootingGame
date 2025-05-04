using UnityEngine;

public class EnemyDamagedState : IEnemeyState
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

    }

    public void EndState()
    {

    }
}
