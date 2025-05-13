using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStatus : MonoBehaviour
{
    public Slider HealthSlider;
    public Slider StaminaSlider;
    public Image UI_HitEffect;
    public PlayerStatus PlayerStat;
    public PlayerController PlayerController;

    public Action OnChangeStatus;

    private Coroutine _runningCoroutine = null;

    [SerializeField]
    private float _alphaReduction = 0.1f;

    void Awake()
    {
        OnChangeStatus += RefreshUI;
    }

    void Start()
    {
        PlayerController.OnAttacked += HitEfftct;
    }

    public void RefreshUI()
    {
        HealthSlider.value = PlayerStat.Health;
        StaminaSlider.value = PlayerStat.Stamina;
    }

    public void HitEfftct(Damage damage)
    {
        UI_HitEffect.color = Color.white;

        
        if(_runningCoroutine != null)
        {
            StopCoroutine(_runningCoroutine);
        }
        _runningCoroutine = StartCoroutine(HitEffectCoroutine(_alphaReduction));
    }

    IEnumerator HitEffectCoroutine(float alphaReduction)
    {

        while(UI_HitEffect.color.a > 0)
        {   
            yield return null;
            UI_HitEffect.color -= new Color(0,0,0,alphaReduction);
        }
    }
}
