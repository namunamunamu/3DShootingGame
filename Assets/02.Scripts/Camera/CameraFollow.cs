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
    }

    private void Update()
    {
        transform.position = _targetTransform.position;
    }

    public void SetCarmeraTransform(ECameraMode cameraMode)
    {
        switch (cameraMode)
        {
            case ECameraMode.FPS:
                _targetTransform = FPSTransform;
                break;

            case ECameraMode.TPS:
                _targetTransform = TPSTransform;
                break;    

            case ECameraMode.Qurter:
                _targetTransform = QurterTransform;
                break;

            default:
                Debug.LogWarning("유효한 카메라 모드가 아닙니다!");
                break;
        }
    }
}
