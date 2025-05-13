using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonBehaviour<PoolManager>
{
    [Header("Enemy Pool")]
    public List<GameObject> EnemyPrefabs;
    public List<int> EnemyPoolSize;
    private List<GameObject>[] _enemiesPools;

    [Header("Boom Pool")]
    public Bomb BombPrefab;
    public int BombPoolSize;

    [SerializeField]
    private List<Bomb> _bombPool;


    [Header("VFX Pool")]
    public List<ParticleSystem> VFXPrefabs;
    public List<int> VFXPoolSize;

    [SerializeField]
    private List<ParticleSystem> _vfxPool;

    private GameObject _player;


    private void Awake()
    {
        CreateBombPool();
        CreateVFXPool();
        CreateEnemyPool();
    }

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }

    private void CreateVFXPool()
    {

        int poolSize = 0;
        for(int i=0; i<VFXPrefabs.Count; i++)
        {
            poolSize += VFXPoolSize[i];
        } 

        _vfxPool = new List<ParticleSystem>(poolSize);

        for(int i=0; i<VFXPrefabs.Count; i++)
        {
            for(int j=0; j < VFXPoolSize[i]; j++)
            {
                ParticleSystem vfx = Instantiate(VFXPrefabs[i], transform);
                _vfxPool.Add(vfx);
                vfx.gameObject.SetActive(false);
            }
        }

    }

    public ParticleSystem GetVFX(string tag)
    {
        foreach(ParticleSystem vfx in _vfxPool)
        {
            if(vfx.tag == tag && vfx.gameObject.activeInHierarchy == false)
            {
                vfx.gameObject.SetActive(true);
                return vfx;
            }
        }

        Debug.LogError($"남은 {tag} 가 없습니다!!");
        return null;
    }

    private void CreateBombPool()
    {
        _bombPool = new List<Bomb>(BombPoolSize);

        for(int i=0; i < BombPoolSize; i++)
        {
            Bomb bomb = Instantiate(BombPrefab, transform);
            bomb.gameObject.SetActive(false);
            _bombPool.Add(bomb);
        }
    }
    
    public Bomb GetBomb()
    {
        foreach(Bomb bomb in _bombPool)
        {
            if(bomb.gameObject.activeInHierarchy == false)
            {
                bomb.gameObject.SetActive(true);
                return bomb;
            }
        }

        Debug.LogWarning("수류탄 풀이 부족합니다!!");
        return null;
    }


    private void CreateEnemyPool()
    {
        _enemiesPools = new List<GameObject>[EnemyPrefabs.Count];
        for(int i=0; i<EnemyPrefabs.Count; i++)
        {
            _enemiesPools[i] = new List<GameObject>(EnemyPoolSize[i]);
        }

        for(int i=0; i<EnemyPrefabs.Count; i++)
        {
            for(int j=0; j < EnemyPoolSize[i]; j++)
            {
                GameObject enemy = Instantiate(EnemyPrefabs[i], transform);
                _enemiesPools[i].Add(enemy);
                enemy.gameObject.SetActive(false);
            }
        }
    }

    public GameObject GetEnemy(Type type)
    {
        int index = 0;
        if(type == typeof(Enemy))
        {
            Debug.Log("Basic");
            index = (int)EEnemyType.Basic;
        }
        if(type == typeof(ChaseEnemy))
        {
            Debug.Log("Chase");
            index = (int)EEnemyType.Chase;
        }

        foreach(GameObject enemy in _enemiesPools[index])
        {
            if(enemy.activeInHierarchy == false)
            {
                enemy.gameObject.SetActive(true);
                enemy.GetComponent<EnemyBase>().Initialize(_player);
                return enemy;
            }
        }

        Debug.LogError($"남은 {type} 가 없습니다!!");
        return null;
    }
}
