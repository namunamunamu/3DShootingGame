using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform FPSTransform;
    public Transform TPSTransform;
    public Transform QurterTransform;


    private Transform _targetTransform;

    void Start()
    {
        _targetTransform = FPSTransform;
        CameraManager.Instance.OnChangeCameraMode += SetCarmeraTransform;
    }

    private void Update()
    {
        transform.position = _targetTransform.position;
    }

    private void SetCarmeraTransform(CameraMode cameraMode)
    {
        switch (cameraMode)
        {
            case CameraMode.FPS:
                _targetTransform = FPSTransform;
                break;

            case CameraMode.TPS:
                _targetTransform = TPSTransform;
                break;    

            case CameraMode.Qurter:
                _targetTransform = QurterTransform;
                break;

            default:
                Debug.LogWarning("유효한 카메라 모드가 아닙니다!");
                break;
        }
    }
}
