using System;
using System.Collections;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject FirePosition;

    public TrailRenderer BulletTrail;
    public float BulletSpeed;
    public int BulletDamage = 10;
    public float BulletKnockBack = 1000f;

    public float ThrowPower = 15f;
    public float MaxThrowPower = 5f;

    private bool _isReloading;
    private float _reloadTimer;

    private float _fireTimer;

    private float _throwTimer;

    public Action OnFire;

    private void Update()
    {
        ThrowGrenade();

        FireBullet();

        Reload();
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
            bomb.transform.position = FirePosition.transform.position;

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

        if(Input.GetMouseButton(0) && _fireTimer >= WeaponManager.Instance.FireTime)
        {
            _isReloading = false;
            _reloadTimer = 0f;
            UI_Manager.Instance.WeaponPanel.OnReload(_isReloading);

            if(WeaponManager.Instance.CurrentBullets <= 0)
            {
                Debug.Log("총알이 없습니다!");
                return;
            }

            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo = new RaycastHit();

            bool isHit = Physics.Raycast(ray, out hitInfo);
            if(isHit)
            {
                TrailRenderer trail = Instantiate(BulletTrail, FirePosition.transform.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hitInfo.point));

                ParticleSystem bulletEffect = PoolManager.Instance.GetVFX("BulletStormVFX");
                bulletEffect.gameObject.transform.position = hitInfo.point;
                bulletEffect.gameObject.transform.forward = hitInfo.normal;
                bulletEffect.Play();

                if(hitInfo.collider.gameObject.CompareTag("Enemy"))
                {
                    Enemy enemy = hitInfo.collider.GetComponent<Enemy>();

                    Damage damage = new Damage(){Value = BulletDamage, KnockBackPower = BulletKnockBack, From = gameObject };

                    enemy.TakeDamage(damage);
                }

                if(hitInfo.collider.gameObject.CompareTag("Barrel"))
                {
                    Barrel enemy = hitInfo.collider.GetComponent<Barrel>();

                    Damage damage = new Damage(){Value = BulletDamage, KnockBackPower = BulletKnockBack, From = gameObject };

                    enemy.TakeDamage(damage);
                }
            }

            WeaponManager.Instance.AddBullet(-1);
            OnFire();
            _fireTimer = 0;
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

             UI_Manager.Instance.WeaponPanel.OnReloadTimerChange(_reloadTimer, WeaponManager.Instance.ReloadTime);

            if(_reloadTimer >= WeaponManager.Instance.ReloadTime)
            {
                WeaponManager.Instance.ReloadMag();
                _isReloading = false;
                _reloadTimer = 0;
                 UI_Manager.Instance.WeaponPanel.OnReload(_isReloading);
                Debug.Log("재장전 완료!");
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

            remainingDistance -= BulletSpeed * Time.deltaTime;

            yield return null;
        }
        trail.transform.position = hitPoint;

        Destroy(trail.gameObject, trail.time);
    }
}
