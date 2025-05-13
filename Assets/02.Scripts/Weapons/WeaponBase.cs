using UnityEngine;

public enum EWeaponType
{
    Melee,
    Gun,
    Throwable
}

public class WeaponBase : MonoBehaviour
{
    public TrailRenderer BulletTrail;

    [SerializeField]
    private WeaponDataSO _weaponData;
    public WeaponDataSO WeaponData => _weaponData;


    [SerializeField]
    private Transform _firePosition;
    public Transform FirePosition => _firePosition;


    public float FireTime => _fireTime;
    public int CurrentBullets => _currentBullets;
    public Damage Damage => _damage;


    private float _fireTime;
    private int _currentBullets;
    private Damage _damage;

    private const int ONEMINUTE = 60;

    private void Start()
    {
        Initalize();
    }

    private void Initalize()
    {
        _damage = new Damage(){ Value = _weaponData.BulletDamage,
                                KnockBackPower = _weaponData.BulletKnockBack,
                                From = gameObject };

        _fireTime = ONEMINUTE / _weaponData.FireRate;
        _currentBullets = _weaponData.MagSize;
        UI_Manager.Instance.WeaponPanel.OnChangeBulletCount(_currentBullets, _weaponData.MagSize);
    }

    public virtual void Reload()
    {
        if(WeaponManager.Instance.CurrentMags <= 0)
        {
            Debug.Log("남은 탄창이 없습니다!!");
        }

        if(WeaponManager.Instance.AddMag(-1))
        {
            _currentBullets = _weaponData.MagSize;
            UI_Manager.Instance.WeaponPanel.OnChangeBulletCount(_currentBullets, _weaponData.MagSize);
        }
    }

    public bool AddBullet(int amount)
    {
        if(_currentBullets + amount > _weaponData.MagSize)
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
        UI_Manager.Instance.WeaponPanel.OnChangeBulletCount(_currentBullets, _weaponData.MagSize);
        return true;
    }
}
