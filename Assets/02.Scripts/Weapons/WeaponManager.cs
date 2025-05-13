using System;
using UnityEngine;

public class WeaponManager : SingletonBehaviour<WeaponManager>
{
    public int MaxGrenades = 3;
    public int MaxMags = 5;

    [SerializeField]
    private int _currentGrenades = 0;
    public int CurrentGrenades => _currentGrenades;

    [SerializeField]
    private int _currentMags = 0;
    public int CurrentMags => _currentMags;


    public Action<int> OnChangeGrenadeCount;
    public Action<int> OnChangeMagCount;
   


    private void Awake()
    {
        _currentGrenades = MaxGrenades;
        _currentMags = MaxMags;
    }

    void Start()
    {
        OnChangeGrenadeCount(_currentGrenades);
        OnChangeMagCount(_currentMags);
    }


    public bool AddGrenadeCount(int amount)
    {
        if(_currentGrenades + amount > MaxGrenades)
        {
            Debug.Log("수류탄이 가득 찼습니다!");
            return false;
        }

        if(_currentGrenades + amount < 0)
        {
            Debug.LogWarning("유효하지 않은 수류탄 사용!!");
            return false;
        }

        _currentGrenades += amount;
        OnChangeGrenadeCount(_currentGrenades);
        return true;
    }

    public bool AddMag(int amount)
    {
        if(_currentMags + amount > MaxMags)
        {
            Debug.Log("탄창주머니가 가득 찼습니다!");
            return false;
        }

        if(_currentMags + amount < 0)
        {
            Debug.LogWarning("유효하지 않은 탄창 사용!!");
            return false;
        }

        _currentMags += amount;
        OnChangeMagCount(_currentMags);
        return true;
    }


    public Vector3 GetRecoil()
    {
        // TODO
        // GetRecoilValue
        float verticalRecoil = 5f;
        float horizontalrecoil = 3f;

        // GetRecoilPattren;( rate : 0 - 1)
        float verticalRecoilRate = 0.2f;
        float horizontailRecoilRate = 0.5f;

        float leftRecoil = horizontalrecoil * horizontailRecoilRate;
        float minVerticalRecoil = verticalRecoil * verticalRecoilRate;

        Vector3 recoil = new Vector3(UnityEngine.Random.Range(-leftRecoil, horizontalrecoil - leftRecoil), UnityEngine.Random.Range(minVerticalRecoil,  verticalRecoil), 0);
        return recoil;
    }
}
