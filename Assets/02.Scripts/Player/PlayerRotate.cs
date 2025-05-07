using Unity.Mathematics;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    
    [SerializeField]
    private Transform _TPSCameraArm;

    [SerializeField]
    private Transform _qurterCameraArm;
    
    public Transform WeaponTranform;

    private ECameraMode _currentCameraMode;
    private CameraController _mainCameraController;
    private CameraRotate _cameraRotate;

    private float _rotationX = 0;
    private float _rotationY = 0;


    void Start()
    {
        _cameraRotate = Camera.main.gameObject.GetComponent<CameraRotate>();
        _mainCameraController = Camera.main.gameObject.GetComponent<CameraController>();
        _mainCameraController.OnChangeCameraMode += SetCurrentCameraMode;
    }

    void Update()
    {
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        _rotationX = _cameraRotate.RotationX;
        _rotationY = _cameraRotate.RotationY;

        // 3. 플레이어를 회전한다.
        transform.eulerAngles = new Vector3(0, _rotationX, 0);
        WeaponTranform.localEulerAngles = new Vector3(_rotationY, 180, 0);

        if(_currentCameraMode == ECameraMode.TPS)
        {
            _TPSCameraArm.localEulerAngles = new Vector3(-_rotationY, 0, 0);
        }

        if(_currentCameraMode == ECameraMode.Qurter)
        {
            _qurterCameraArm.rotation = quaternion.Euler(0,0,0);
        }
    }

    private void SetCurrentCameraMode(ECameraMode cameraMode)
    {
        _currentCameraMode = cameraMode;
    }
}

