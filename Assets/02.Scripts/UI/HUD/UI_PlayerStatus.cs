using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStatus : MonoBehaviour
{
    public Slider HealthSlider;
    public Slider StaminaSlider;
    public PlayerStatus PlayerStat;

    public Action OnChangeStatus;

    void Awake()
    {
        OnChangeStatus += RefreshUI;
    }
    
    public void RefreshUI()
    {
        HealthSlider.value = PlayerStat.Health;
        StaminaSlider.value = PlayerStat.Stamina;
    }
}
