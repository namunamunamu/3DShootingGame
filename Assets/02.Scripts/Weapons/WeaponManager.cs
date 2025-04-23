using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponManager : SingletonBehaviour<WeaponManager>
{
    public int MaxGrenades = 3;
    public int MaxMags = 5;
    public int MagSize = 50;
    public int FireRate = 600;

    [SerializeField]
    private float _fireTime;
    public float FireTime => _fireTime;

    [SerializeField]
    private float _reloadTime = 2f;
    public float ReloadTime => _reloadTime;

    [SerializeField]
    private int _currentGrenades = 0;
    public int CurrentGrenades => _currentGrenades;

    [SerializeField]
    private int _currentBullets = 0;
    public int CurrentBullets => _currentBullets;

    [SerializeField]
    private int _currentMags = 0;
    public int CurrentMags => _currentMags;


    public Action<int> OnChangeGrenadeCount;
    public Action<int> OnChangeMagCount;
    public Action<int, int> OnChangeBulletCount;

    private const float ONEMINUTE = 60f;

    private void Awake()
    {
        _currentBullets = MagSize;
        _currentGrenades = MaxGrenades;
        _currentMags = MaxMags;

        SetFireTime();
    }

    void Start()
    {
        OnChangeBulletCount(_currentBullets, MagSize);
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

    public bool AddBullet(int amount)
    {
        if(_currentBullets + amount > MagSize)
        {
            Debug.Log("탄창이 가득 찼습니다!");
            return false;
        }

        if(_currentBullets + amount < 0)
        {
            Debug.LogWarning("유효하지 않은 총알 사용!!");
            return false;
        }

        _currentBullets += amount;
        OnChangeBulletCount(_currentBullets, MagSize);
        return true;
    }

    public bool ReloadMag()
    {
        if(_currentMags <= 0)
        {
            _currentMags = 0;
            Debug.Log("남은 탄창이 없습니다!!");
            return false;
        }

        if(AddMag(-1))
        {
            _currentBullets = MagSize;
            OnChangeBulletCount(_currentBullets, MagSize);
            return true;
        }

        return false;
    }

    private void SetFireTime()
    {
        _fireTime = ONEMINUTE / FireRate;
    }

    public float[][] GetRecoil(float verticalRecoil, float horizontalrecoil)
    {
        float verticalRecoilRate = 0.7f;
        float horizontailRecoilRate = 0.2f;

        int leftRecoil = (int)(verticalRecoil / verticalRecoilRate);
        int minHorizontailRecoil = (int)(horizontalrecoil / horizontailRecoilRate);

        float[] verticalRecoilRange = {-leftRecoil, verticalRecoil - leftRecoil};
        float[] horizontalRecoilRange = {minHorizontailRecoil, horizontalrecoil};

        float[][] Recoils = {verticalRecoilRange, horizontalRecoilRange};

        return Recoils;
    }
}
