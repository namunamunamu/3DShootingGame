using TMPro;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // 2. 오른쪽 버튼 입력 받기
    // 3. 발사 위치에 수류탄 생성하기
    // 4. 생성된 수류탄을 카메라 방향으로 물리적으로 던지기

    // 필요 속성
    // -발사 위치
    public GameObject FirePosition;
    // -폭탄 프리팹

    public float ThrowPower = 15f;
    public float MaxThrowPower = 5f;

    public ParticleSystem BulletEffect;

    private bool _isReloading;
    private float _reloadTimer;

    private float _fireTimer;

    private float _throwTimer;


    // 목표: 마우스의 왼쪽 버튼을 누르면 카메라가 바라보는 방향으로 총을 발사하고 싶다.
    // 총알 발사(레이저 방식)
        // 1. 왼쪽버튼 입력받기
        // 2. 레이를 생성하고 발사위치와 진행방향을 설정
        // 3. 레이와 부딛히 물체의 정보를 저장할 변수를 생성
        // 4. 레이를 발사한다음,                ㄴ에 데이터가 있다면 피격이펙트 생성

        // Ray: 레이저(시작위치, 방향)
        // RayCast: 레이저를 발사
        // RayCastHit: 레이저가 물체와 부딛혔다면 그 정보를 저장하는 구조체


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

            // GameObject bomb = Instantiate(BombPrefab);
            Bomb bomb = PoolManager.Instance.GetBomb();
            bomb.transform.position = FirePosition.transform.position;

            Rigidbody bombRigidBody = bomb.GetComponent<Rigidbody>();
            bombRigidBody.AddForce(Camera.main.transform.forward * (ThrowPower + _throwTimer), ForceMode.Impulse);
            Debug.Log(ThrowPower + _throwTimer);
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
            UI_WeaponPanel.Instance.OnReload(_isReloading);

            if(WeaponManager.Instance.CurrentBullets <= 0)
            {
                Debug.Log("총알이 없습니다!");
                return;
            }

            Ray ray = new Ray(FirePosition.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo = new RaycastHit();

            bool isHit = Physics.Raycast(ray, out hitInfo);
            if(isHit)
            {
                BulletEffect.transform.position = hitInfo.point;
                BulletEffect.transform.forward = hitInfo.normal;
                BulletEffect.Play();
            }

            WeaponManager.Instance.AddBullet(-1);
            _fireTimer = 0;
        }
    }

    private void Reload()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("재장전!");
            _isReloading = true;
            UI_WeaponPanel.Instance.OnReload(_isReloading);
        }

        if(_isReloading)
        {
            _reloadTimer += Time.deltaTime;

            UI_WeaponPanel.Instance.OnReloadTimerChange(_reloadTimer, WeaponManager.Instance.ReloadTime);

            if(_reloadTimer >= WeaponManager.Instance.ReloadTime)
            {
                WeaponManager.Instance.ReloadMag();
                _isReloading = false;
                _reloadTimer = 0;
                UI_WeaponPanel.Instance.OnReload(_isReloading);
                Debug.Log("재장전 완료!");
            }
        }
    }
}
