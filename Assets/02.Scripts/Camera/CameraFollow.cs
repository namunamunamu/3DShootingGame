using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform TargetTransfrom;

    private void Update()
    {
        // 보간 기법 (interpolation, smoothing)
        transform.position =  TargetTransfrom.position;
    }
}
