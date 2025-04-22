using UnityEngine;


public class CameraRotate : MonoBehaviour
{
    // 카메라 회전 스크립트
    // 목표: 마우스를 조작하면 카메라를 그 방향으로 회전시키고 싶다.
    [SerializeField]
    private Transform _playerTransfrom;

    // 카메라 각도는 0도에서부터 시작한다고 기준을 세운다.
    private float _rotationSpeed;
    private float _rotationX = 0;
    private float _rotationY = 0;

    private CameraMode _currentCameraMode;



    void Start()
    {
        CameraManager.Instance.OnChangeCameraMode += SetCurrentCameraMode;
        CameraManager.Instance.OnChangeCameraRotationSpeed += SetCameraRotationSpeed;
        _rotationSpeed = CameraManager.Instance.CameraRotationSpeed;

        if(_currentCameraMode != CameraMode.Qurter)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Update()
    {
        if(_currentCameraMode != CameraMode.Qurter)
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
        _rotationX += mouseX * _rotationSpeed * Time.deltaTime;
        _rotationY += mouseY * _rotationSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

        // 3. 카메라를 회전한다.
        transform.eulerAngles = new Vector3(-_rotationY, _rotationX, 0);
    }

    private void QurterCameraRotate()
    {
        Vector3 dir = _playerTransfrom.position - transform.position;
        dir = Vector3.Normalize(dir); 

        Quaternion targetRotation = Quaternion.LookRotation(dir);

        transform.rotation = targetRotation;
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
