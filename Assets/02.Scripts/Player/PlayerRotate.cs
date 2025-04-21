using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float RotationSpeed = 150f; // 카메라와 회전속도가 똑같아야 한다.
    public GameObject CameraBoom;

    private float _rotationX = 0;
    private float _rotationY = 0;

    void Update()
    {
        SetMouseSensitive();
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        // 구현 순서
        // 1. 마우스 입력을 받는다.(마우스 커서의 움직임 방향)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 2. 마우스 입력으로부터 회전할 양만큼 누적시킨다.
        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        _rotationY += mouseY * RotationSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

        // 3. 카메라를 회전한다.
        transform.eulerAngles = new Vector3(0, _rotationX, 0);
        CameraBoom.transform.localEulerAngles = new Vector3(-_rotationY, 0, 0);
    }

    private void SetMouseSensitive()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            RotationSpeed -= 10;
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            RotationSpeed += 10;
        }
    }
}

