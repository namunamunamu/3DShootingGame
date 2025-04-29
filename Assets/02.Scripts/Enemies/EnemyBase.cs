using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public EnemyDataSO EnemyData;

    private Dictionary<EEnemyState, IEnemeyState> _enemyStateDict = new Dictionary<EEnemyState, IEnemeyState>();

    private EnemyStateMachine _enemyFSM;
    private GameObject _player;

    private void Awake()
    {
        _enemyFSM = new EnemyStateMachine(this);
        _enemyStateDict.Add(EEnemyState.Idle, new EnemyIdleState());
        _enemyStateDict.Add(EEnemyState.Patrol, new EnemyIdleState());
        _enemyStateDict.Add(EEnemyState.Attack, new EnemyIdleState());
        _enemyStateDict.Add(EEnemyState.Trace, new EnemyIdleState());
        _enemyStateDict.Add(EEnemyState.Return, new EnemyIdleState());
        _enemyStateDict.Add(EEnemyState.Die, new EnemyIdleState());
    }

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }
    private void Update()
    {
        if(Vector3.Distance(transform.position, _player.transform.position) < EnemyData.FindDistance)
        {
            _enemyFSM.SetState(_enemyStateDict[EEnemyState.Trace]);
        }

        _enemyFSM.DoState();
    }
}
