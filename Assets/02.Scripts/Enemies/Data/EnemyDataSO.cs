using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Scriptable Objects/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    public int Health;
    public float MoveSpeed;

    public float PatrolDistance;
    public int PatrolPointCount;

    public float FindDistance;
    public float ReturnDistance;
    public float AttackDistance;
    public float AttackCoolTime;

    public float DamageTime;
    public float DeathTime;
    public float IdleTime;    
}
