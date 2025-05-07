using UnityEngine;

public class EnemyDieState : IEnemeyState
{
    private  EnemyBase _enemy;
    private GameObject _player; 
    private EnemyStateMachine _stateMachine;

    private float _deathTimer;

    public void InitializeState(EnemyStateMachine stateMachine, EnemyBase enemy, GameObject player)
    {
        _enemy = enemy;
        _player = player;
        _stateMachine = stateMachine;
        _deathTimer = 0f;
    }

    public void ExcuteState()
    {
        _deathTimer += Time.deltaTime;
        if(_deathTimer >= _enemy.EnemyData.DeathTime)
        {
            _deathTimer = 0f;
            _enemy.gameObject.SetActive(false);
        }
    }

    public void EndState()
    {
        _deathTimer = 0f;
    }
}
