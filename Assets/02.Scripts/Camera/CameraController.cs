using System;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public ECameraMode CurrentCameraMode = ECameraMode.FPS;

    [SerializeField]
    private float _cameraRotationSpeed = 150f;
    public float CameraRotationSpeed => _cameraRotationSpeed;

    private CameraFollow _cameraFollow;
    private CameraRotate _cameraRotate;


    public Action<ECameraMode> OnChangeCameraMode;
    public Action<float> OnChangeCameraRotationSpeed; 


    private void Start()
    {
        _cameraFollow = gameObject.GetComponent<CameraFollow>();
        _cameraRotate = gameObject.GetComponent<CameraRotate>();
        _cameraRotate.RotationSpeed = _cameraRotationSpeed;
        _cameraRotate.CurrentCamerMode = CurrentCameraMode;
    }

    private void Update()
    {
        SetCamerMode();
        SetMouseSensitive();
    }

    private void SetCamerMode()
    {
        if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            CurrentCameraMode = ECameraMode.FPS;
            OnChangeCameraMode(CurrentCameraMode);

            _cameraRotate.CurrentCamerMode = CurrentCameraMode;
            _cameraFollow.SetCarmeraTransform(CurrentCameraMode);
        }

        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            CurrentCameraMode = ECameraMode.TPS;
            OnChangeCameraMode(CurrentCameraMode);

            _cameraRotate.CurrentCamerMode = CurrentCameraMode;
            _cameraFollow.SetCarmeraTransform(CurrentCameraMode);
        }

        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            CurrentCameraMode = ECameraMode.Qurter;
            OnChangeCameraMode(CurrentCameraMode);

            _cameraRotate.CurrentCamerMode = CurrentCameraMode;
            _cameraFollow.SetCarmeraTransform(CurrentCameraMode);
        }
    }

    private void SetMouseSensitive()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            _cameraRotationSpeed -= 10;
            _cameraRotate.RotationSpeed = _cameraRotationSpeed;
            OnChangeCameraRotationSpeed(_cameraRotationSpeed);
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            _cameraRotationSpeed += 10;
            _cameraRotate.RotationSpeed = _cameraRotationSpeed;
            OnChangeCameraRotationSpeed(_cameraRotationSpeed);
        }
    }
}
