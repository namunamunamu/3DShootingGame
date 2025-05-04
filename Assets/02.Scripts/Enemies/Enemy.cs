using UnityEngine;
using System.Collections.Generic;

public class Enemy : EnemyBase
{   
    public List<Vector3> PatrolPoints =>_patrolPoints;
    public int NextPointIndex {get; set;}


    private List<Vector3> _patrolPoints = new List<Vector3>();


    private void Awake()
    {
        InitializePartolPoint();
    }

    protected override void SetStateDictionary()
    {
        base.SetStateDictionary();

        EnemyStateDict.Add(EEnemyState.Idle, new EnemyIdleState());
        EnemyStateDict.Add(EEnemyState.Trace, new EnemyTraceState());
        EnemyStateDict.Add(EEnemyState.Patrol, new EnemyPatrolState());
        EnemyStateDict.Add(EEnemyState.Return, new EnemyReturnState());
    }

    public void InitializePartolPoint()
    {
        _patrolPoints.Clear();
        _patrolPoints.Add(InitialPosition);
        for(int i=1; i< EnemyData.PatrolPointCount; i++)
        {
            _patrolPoints.Add(new Vector3(Random.Range(InitialPosition.x - EnemyData.PatrolDistance, InitialPosition.x + EnemyData.PatrolDistance),
                                         1,
                                         Random.Range(InitialPosition.z - EnemyData.PatrolDistance, InitialPosition.z + EnemyData.PatrolDistance)));
        }
    }
}
