using UnityEngine;


public class CameraRotate : MonoBehaviour
{
    // 카메라 회전 스크립트
    // 목표: 마우스를 조작하면 카메라를 그 방향으로 회전시키고 싶다.

    public float RotationSpeed{ set{_rotationSpeed = value;}}
    public ECameraMode CurrentCamerMode{set{_currentCameraMode = value;}}

    public float RotationX => _rotationX;
    public float RotationY => _rotationY;

    [SerializeField]
    private Transform _playerTransfrom;

    // 카메라 각도는 0도에서부터 시작한다고 기준을 세운다.
    private float _rotationSpeed;
    private float _rotationX = 0;
    private float _rotationY = 0;

    private ECameraMode _currentCameraMode;
    private Vector3 _recoil;



    void Start()
    {
        if(_currentCameraMode != ECameraMode.Qurter)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        GameObject.FindWithTag("Player").GetComponent<PlayerFire>().OnFire += RotateByRecoil;
    }

    private void Update()
    {
        if(_currentCameraMode != ECameraMode.Qurter)
        {
            RotateCamera();
        }
        else
        {
            QurterCameraRotate();
        }
    }

    private void RotateCamera()
    {
        // 구현 순서
        // 1. 마우스 입력을 받는다.(마우스 커서의 움직임 방향)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 2. 마우스 입력으로부터 회전할 양만큼 누적시킨다.
        _rotationX += _recoil.x + (mouseX * _rotationSpeed * Time.deltaTime);
        _rotationY += -_recoil.y + (mouseY * _rotationSpeed * Time.deltaTime);
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

        // 3. 카메라를 회전한다.
        transform.eulerAngles = new Vector3(-_rotationY, _rotationX, 0);
        _recoil = Vector3.zero;
    }

    private void QurterCameraRotate()
    {
        Vector3 dir = (_playerTransfrom.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = targetRotation;
    }

    private void RotateByRecoil()
    {
        float[][] recoils = WeaponManager.Instance.GetRecoil(3, 1);

        _recoil = new Vector3(Random.Range(recoils[1][0], recoils[1][1]), Random.Range(recoils[0][0], recoils[0][1]), 0);
    }
}
