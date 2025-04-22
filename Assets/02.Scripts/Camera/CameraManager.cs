using System;
using System.Xml.Serialization;
using UnityEngine;

public enum CameraMode
{
    FPS,
    TPS,
    Qurter
}


public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    public CameraMode CurrentCameraMode = CameraMode.FPS;

    [SerializeField]
    private float _cameraRotationSpeed = 150f;
    public float CameraRotationSpeed => _cameraRotationSpeed;

    public Action<CameraMode> OnChangeCameraMode;
    public Action<float> OnChangeCameraRotationSpeed;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
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
            CurrentCameraMode = CameraMode.FPS;
            OnChangeCameraMode(CurrentCameraMode);
        }

        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            CurrentCameraMode = CameraMode.TPS;
            OnChangeCameraMode(CurrentCameraMode);
        }

        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            CurrentCameraMode = CameraMode.Qurter;
            OnChangeCameraMode(CurrentCameraMode);
        }
    }

    private void SetMouseSensitive()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            _cameraRotationSpeed -= 10;
            OnChangeCameraRotationSpeed(_cameraRotationSpeed);
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            _cameraRotationSpeed += 10;
            OnChangeCameraRotationSpeed(_cameraRotationSpeed);
        }
    }
}
