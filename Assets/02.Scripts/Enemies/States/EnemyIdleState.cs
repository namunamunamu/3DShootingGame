using UnityEngine;

public class EnemyIdleState : MonoBehaviour, IEnemeyState
{
    private  EnemyBase _enemy;

    private float _idleTimer;

    public void InitializeState(EnemyBase enemy)
    {
        _enemy = enemy;
    }

    public void DoState()
    {
        _idleTimer += Time.deltaTime;
        if(_idleTimer >= _enemy.EnemyData.IdleTime)
        {
            Debug.Log("상태전환: Idle -> Patrol");
            CurrentState = EEnemyState.Patrol;
            _idleTimer = 0f;
            return;
        }
    }

    public void EndState()
    {
        
    }
}
