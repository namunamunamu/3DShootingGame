using Unity.Mathematics;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    
    [SerializeField]
    private Transform _TPSCameraArm;
    [SerializeField]
    private Transform _qurterCameraArm;


    private CameraMode _currentCameraMode;
    private float _rotationSpeed; // 카메라와 회전속도가 똑같아야 한다.

    private float _rotationX = 0;
    private float _rotationY = 0;


    void Start()
    {
        CameraManager.Instance.OnChangeCameraMode += SetCurrentCameraMode;
        CameraManager.Instance.OnChangeCameraRotationSpeed += SetCameraRotationSpeed;
        _rotationSpeed = CameraManager.Instance.CameraRotationSpeed;
    }

    void Update()
    {
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        // 구현 순서
        // 1. 마우스 입력을 받는다.(마우스 커서의 움직임 방향)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 2. 마우스 입력으로부터 회전할 양만큼 누적시킨다.
        _rotationX += mouseX * _rotationSpeed * Time.deltaTime;
        _rotationY += mouseY * _rotationSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

        // 3. 플레이어를 회전한다.
        transform.eulerAngles = new Vector3(0, _rotationX, 0);

        if(_currentCameraMode == CameraMode.TPS)
        {
            _TPSCameraArm.localEulerAngles = new Vector3(-_rotationY, 0, 0);
        }

        if(_currentCameraMode == CameraMode.Qurter)
        {
            _qurterCameraArm.rotation = quaternion.Euler(0,0,0);
        }
    }

    private void SetCurrentCameraMode(CameraMode cameraMode)
    {
        _currentCameraMode = cameraMode;
    }

    private void SetCameraRotationSpeed(float rotationSpeed)
    {
        _rotationSpeed = rotationSpeed;
    }
}

