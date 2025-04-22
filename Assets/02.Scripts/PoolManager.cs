using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    public Bomb BombPrefab;
    public int BombPoolSize;

    [SerializeField]
    private List<Bomb> _bombPool;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CreateBombPool();
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


}
