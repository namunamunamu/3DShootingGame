using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float MaxDistance;
    public float MinSpawnTime;
    public float MaxSpawnTime;
    public int MinSpawnAmount;
    public int MaxSpanwAmount;

    private float _spawnTime;
    private float _spawnTimer;
    private int _spawnAmount;


    private void Awake()
    {
        SetRandomTime();
        SetRandomAmount();
    }

    private void Update()
    {
        _spawnTimer += Time.deltaTime;
        if(_spawnTimer >= _spawnTime)
        {
            
            for(int i=0; i<_spawnAmount; i++)
            {
                RandomSpawn();
            }

            _spawnTimer = 0f;
            SetRandomTime();
            SetRandomAmount();
        }
    }


    private void SetRandomTime()
    {
        _spawnTime = Random.Range(MinSpawnTime, MaxSpawnTime);
    }

    private void SetRandomAmount()
    {
        _spawnAmount = Random.Range(MinSpawnAmount, MaxSpanwAmount);
    }

    private void RandomSpawn()
    {
        GameObject enemy;

        float spawnChance = Random.Range(0f, 1f);
        if(spawnChance <= 0.3)
        {
            enemy = PoolManager.Instance.GetEnemy(typeof(ChaseEnemy));
        }
        else
        {
            enemy = PoolManager.Instance.GetEnemy(typeof(Enemy));
        }

        Vector3 randomPosition = new Vector3(Random.Range(0f, MaxDistance), 1f, Random.Range(0f, MaxDistance));
        enemy.transform.localPosition = randomPosition;
    }
}
