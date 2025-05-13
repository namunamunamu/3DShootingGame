using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "Scriptable Objects/WeaponDataSO")]
public class WeaponDataSO : ScriptableObject
{
    public EWeaponType WeaponType;

    public int FireRate;
    public int MagSize;
    public float ReloadTime;


    public int BulletDamage;
    public float BulletKnockBack;
    public float BulletSpeed;
}
