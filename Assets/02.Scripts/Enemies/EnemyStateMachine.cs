
public class EnemyStateMachine
{
    private IEnemeyState _currentState;
    private EnemyBase _enemy;

    public EnemyStateMachine(EnemyBase enemy)
    {
        _enemy = enemy;
    }
  
    public void SetState(IEnemeyState nextState)
    {
        if(_currentState == nextState)
        {
            return;
        }

        _currentState.ExitState();
        _currentState = nextState;
        _currentState.InitializeState(_enemy);
    }

    public void DoState()
    {
        _currentState.DoState();
    }
}
