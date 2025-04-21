using System;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance;

    public float MaxStamina;

    [SerializeField]
    private float _stamina = 0;

    public float Stamina => _stamina;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetStamina(MaxStamina);
    }

    public void SetStamina(float value)
    {
        _stamina += value;
        if(_stamina <= 0)
        {
            _stamina = 0;
        }

        if(_stamina >= MaxStamina)
        {
            _stamina = MaxStamina;
        }

        UI_PlayerStatus.Instance.OnChangeStamina(_stamina);
    }
}
