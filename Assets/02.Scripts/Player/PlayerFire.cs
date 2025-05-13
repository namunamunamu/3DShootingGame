using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public List<WeaponBase> Weapons;

    public float ThrowPower = 15f;
    public float MaxThrowPower = 5f;

    private WeaponBase _currentWeapon;

    private bool _isReloading;
    private float _reloadTimer;

    private float _fireTimer;

    private float _throwTimer;

    public Action OnFire;

    private void Start()
    {
        _currentWeapon = Weapons[0];
    }

    private void Update()
    {
        SwitchWeapon();

        ThrowGrenade();

        FireBullet();

        Reload();
    }

    private void SwitchWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            // knife
            _currentWeapon = Weapons[0];
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Gun
            _currentWeapon = Weapons[1];
        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Grenade
            _currentWeapon = Weapons[2];
        }
    }

    private void ThrowGrenade()
    {
        if(Input.GetMouseButton(1))
        {
            _throwTimer += Time.deltaTime;
            if(_throwTimer >= MaxThrowPower)
            {
                _throwTimer = MaxThrowPower;
            }
        }

        if(Input.GetMouseButtonUp(1))
        {
            if(WeaponManager.Instance.CurrentGrenades <= 0)
            {
                Debug.Log("수류탄이 없습니다!!");
                return;
            }

            Bomb bomb = PoolManager.Instance.GetBomb();
            bomb.transform.position = _currentWeapon.FirePosition.position;

            Rigidbody bombRigidBody = bomb.GetComponent<Rigidbody>();
            bombRigidBody.AddForce(Camera.main.transform.forward * (ThrowPower + _throwTimer), ForceMode.Impulse);
            bombRigidBody.AddTorque(Vector3.one);

            WeaponManager.Instance.AddGrenadeCount(-1);

            _throwTimer = 0f;
        }
    }


    private void FireBullet()
    {
        _fireTimer += Time.deltaTime;

        if(Input.GetMouseButton(0) && _fireTimer >= _currentWeapon.FireTime)
        {
            if(_currentWeapon.CurrentBullets <= 0)
            {
                Debug.Log("총알이 없습니다!");
                return;
            }

            if(_isReloading)
            {
                _isReloading = false;
                _reloadTimer = 0f;
                UI_Manager.Instance.WeaponPanel.OnReload(_isReloading);
            }

            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo = new RaycastHit();

            bool isHit = Physics.Raycast(ray, out hitInfo);
            if(isHit)
            {
                if(hitInfo.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
                {
                    damageable.TakeDamage(_currentWeapon.Damage);
                }

                TrailRenderer trail = Instantiate(_currentWeapon.BulletTrail, _currentWeapon.FirePosition.transform.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hitInfo.point));

                ParticleSystem bulletEffect = PoolManager.Instance.GetVFX("BulletStormVFX");
                bulletEffect.gameObject.transform.position = hitInfo.point;
                bulletEffect.gameObject.transform.forward = hitInfo.normal;
                bulletEffect.Play();
            }
            _currentWeapon.AddBullet(-1);
            _fireTimer = 0;

            OnFire();
        }
    }

    private void Reload()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("재장전!");
            _isReloading = true;

            UI_Manager.Instance.WeaponPanel.OnReload(_isReloading);
        }

        if(_isReloading)
        {
            _reloadTimer += Time.deltaTime;

            UI_Manager.Instance.WeaponPanel.OnReloadTimerChange(_reloadTimer, _currentWeapon.WeaponData.ReloadTime);

            if(_reloadTimer >= _currentWeapon.WeaponData.ReloadTime)
            {
                _currentWeapon.Reload();

                _isReloading = false;
                _reloadTimer = 0;
                Debug.Log("재장전 완료!");

                UI_Manager.Instance.WeaponPanel.OnReload(_isReloading);
            }
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPoint)
    {
        Vector3 startPosition = trail.transform.position;
        float distance = Vector3.Distance(trail.transform.position, hitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hitPoint, 1 - (remainingDistance / distance));

            remainingDistance -= _currentWeapon.WeaponData.BulletSpeed * Time.deltaTime;

            yield return null;
        }
        trail.transform.position = hitPoint;

        Destroy(trail.gameObject, trail.time);
    }
}
