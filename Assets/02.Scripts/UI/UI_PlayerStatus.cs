using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStatus : MonoBehaviour
{
    public static UI_PlayerStatus Instance;

    public Slider StaminaSlider;

    public Action<float> OnChangeStamina;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        OnChangeStamina += RefreshStamina;
    }
    
    public void RefreshStamina(float staminaValue)
    {
        StaminaSlider.value = staminaValue;
    }
}
