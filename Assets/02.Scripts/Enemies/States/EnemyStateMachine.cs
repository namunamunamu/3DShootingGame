
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EEnemyState CurrentStateType => _currentStateType;

    private Dictionary<EEnemyState, IEnemeyState> _stateDict;
    private EnemyBase _enemy;
    private GameObject _player;
    private IEnemeyState _currentState;
    private EEnemyState _currentStateType;


    public EnemyStateMachine(EnemyBase enemy, Dictionary<EEnemyState, IEnemeyState> stateDict)
    {
        _stateDict = stateDict;
        _enemy = enemy;
        _currentStateType = EEnemyState.Idle;
        _currentState = new EnemyIdleState();
    }

    public void SetPlayer(GameObject player)
    {
        _player = player;
    }
  
    public void SetState(EEnemyState nextState)
    {
        if(!_stateDict.TryGetValue(nextState, out IEnemeyState newState))
        {
            Debug.LogWarning("유효하지 않는 State!");
            return;
        }


        if(_currentState == newState)
        {
            return;
        }
        _currentStateType = nextState;
        _enemy.OnChangeState();

        _currentState.EndState();
        _currentState = newState;
        _currentState.InitializeState(this, _enemy, _player);
    }

    public void ExcuteState()
    {
        _currentState.ExcuteState();
    }
}
