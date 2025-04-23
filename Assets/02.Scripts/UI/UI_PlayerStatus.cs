using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStatus : MonoBehaviour
{
    public Slider StaminaSlider;

    public Action<float> OnChangeStamina;

    void Awake()
    {
        OnChangeStamina += RefreshStamina;
    }
    
    public void RefreshStamina(float staminaValue)
    {
        StaminaSlider.value = staminaValue;
    }
}
