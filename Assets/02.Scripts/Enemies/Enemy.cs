using UnityEngine;
using System.Collections.Generic;

public class Enemy : EnemyBase
{   
    public List<Vector3> PatrolPoints =>_patrolPoints;
    public int NextPointIndex {get; set;}


    private List<Vector3> _patrolPoints = new List<Vector3>();


    protected override void Awake()
    {
        base.Awake();
        InitializePartolPoint();
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
