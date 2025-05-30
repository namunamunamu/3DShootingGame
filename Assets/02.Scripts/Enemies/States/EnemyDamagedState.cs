using System.Collections;
using UnityEngine;

public class EnemyDamagedState : IEnemeyState
{
    private  EnemyBase _enemy;
    private GameObject _player; 
    private EnemyStateMachine _stateMachine;

    private float _damagedTimer;

    public void InitializeState(EnemyStateMachine stateMachine, EnemyBase enemy, GameObject player)
    {
        _enemy = enemy;
        _player = player;
        _stateMachine = stateMachine;
        _damagedTimer = 0f;
    }

    public void ExcuteState()
    {
        _damagedTimer += Time.deltaTime;
        if(_damagedTimer >= _enemy.EnemyData.DamageTime)
        {
            Debug.Log("상태전환: Damaged -> Trace");
            _stateMachine.SetState(EEnemyState.Trace);
            _damagedTimer = 0f;
        }
    }

    public void EndState()
    {
        _damagedTimer = 0f;
    }
}
