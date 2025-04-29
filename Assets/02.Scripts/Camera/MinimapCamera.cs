using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform Target;
    public float YOffset = 10f;

    public int MaxZoomIn = 3;
    public int MinZoomOut = 10;
    public int ZoomStep = 5;

    private Camera _minimapCamera;

    void Awake()
    {
        _minimapCamera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        Vector3 newPoistion = Target.position;
        newPoistion.y += YOffset;

        transform.position = newPoistion;

        Vector3 newEulerAngles = Target.eulerAngles;
        newEulerAngles.x = 90;
        newEulerAngles.z = 0;

        transform.eulerAngles = newEulerAngles;

        if(Input.GetKeyDown(KeyCode.Equals))
        {
            _minimapCamera.orthographicSize += ZoomStep;
            if(_minimapCamera.orthographicSize >= MinZoomOut)
            {
                _minimapCamera.orthographicSize = MinZoomOut;
            }
        }

        if(Input.GetKeyDown(KeyCode.Minus))
        {
            _minimapCamera.orthographicSize -= ZoomStep;
            if(_minimapCamera.orthographicSize <= MaxZoomIn)
            {
                _minimapCamera.orthographicSize = MaxZoomIn;
            }
        }
    }
}
