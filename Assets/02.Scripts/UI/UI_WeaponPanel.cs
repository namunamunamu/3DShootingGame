using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponPanel : MonoBehaviour
{
    public TextMeshProUGUI ThrowableText;
    public TextMeshProUGUI BulletText;
    public TextMeshProUGUI MagText;

    public Slider ReloadSlider;

    public GameObject ThrowablePlaceHolderPrefab;

    public Image PrimaryWeaponIcon;
    public Image MagIcon;
    public Image BulletPanelBackground;


    public Transform ThrowablePanel;

    public Action<bool> OnReload;
    public Action<float, float> OnReloadTimerChange;

    private void Start()
    {
        WeaponManager.Instance.OnChangeMagCount += RefreshMagText;
        WeaponManager.Instance.OnChangeGrenadeCount += RefreshGrenadePanel;
        WeaponManager.Instance.OnChangeBulletCount += RefreshBulletText;
        OnReload += SetReloadSliderActive;
        OnReloadTimerChange += RefreshReloadSlider;
        OnReload(false);
    }

    public void SetReloadSliderActive(bool isActive)
    {
        ReloadSlider.gameObject.SetActive(isActive);
    }

    public void RefreshReloadSlider(float value, float maxValue)
    {
        ReloadSlider.maxValue = maxValue;
        ReloadSlider.value = value;
    }

    public void RefreshGrenadePanel(int amount)
    {
        ThrowableText.text = $" x{amount}";
    }

    public void RefreshMagText(int amount)
    {
        MagText.text = $"x{amount}";
    }

    public void RefreshBulletText(int amount, int maxBullet)
    {
        BulletText.text = $"{amount} / {maxBullet}";
    }

}
