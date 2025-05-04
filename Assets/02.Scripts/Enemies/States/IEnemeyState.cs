
using UnityEngine;

public interface IEnemeyState
{
    public void InitializeState(EnemyStateMachine stateMachine, EnemyBase enemy, GameObject player);

    public void ExcuteState();

    public void EndState();
}
