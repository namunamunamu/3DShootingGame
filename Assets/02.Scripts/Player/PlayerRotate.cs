using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    
    [SerializeField]
    private Transform _TPSCameraArm;
    [SerializeField]
    private Transform _qurterCameraArm;

    private PlayerFire _playerFire;
    private CameraController _mainCameraController;
    private ECameraMode _currentCameraMode;
    private float _rotationSpeed; // 카메라와 회전속도가 똑같아야 한다.
    
    private CameraRotate _cameraRotate;



    private float _rotationX = 0;
    private float _rotationY = 0;

    private Vector3 _recoil;


    void Start()
    {
        _playerFire = gameObject.GetComponent<PlayerFire>();
        _cameraRotate = Camera.main.gameObject.GetComponent<CameraRotate>();
        _mainCameraController = Camera.main.gameObject.GetComponent<CameraController>();
        _mainCameraController.OnChangeCameraMode += SetCurrentCameraMode;
        _mainCameraController.OnChangeCameraRotationSpeed += SetCameraRotationSpeed;
        _rotationSpeed = _mainCameraController.CameraRotationSpeed;
        _playerFire.OnFire += RotateByRecoil;
    }

    void Update()
    {
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        // // 구현 순서
        // // 1. 마우스 입력을 받는다.(마우스 커서의 움직임 방향)
        // float mouseX = Input.GetAxis("Mouse X");
        // float mouseY = Input.GetAxis("Mouse Y");

        // // RotateByRecoil();

        // // 2. 마우스 입력으로부터 회전할 양만큼 누적시킨다.
        // _rotationX += _recoil.x + (mouseX * _rotationSpeed * Time.deltaTime);
        // _rotationY += -_recoil.y + (mouseY * _rotationSpeed * Time.deltaTime);
        // _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);
        _rotationX = _cameraRotate.RotationX;
        _rotationY = _cameraRotate.RotationY;



        // 3. 플레이어를 회전한다.
        transform.eulerAngles = new Vector3(0, _rotationX, 0);

        if(_currentCameraMode == ECameraMode.TPS)
        {
            _TPSCameraArm.localEulerAngles = new Vector3(-_rotationY, 0, 0);
        }

        if(_currentCameraMode == ECameraMode.Qurter)
        {
            _qurterCameraArm.rotation = quaternion.Euler(0,0,0);
        }

        _recoil = Vector3.zero;
    }

    private void SetCurrentCameraMode(ECameraMode cameraMode)
    {
        _currentCameraMode = cameraMode;
    }

    private void SetCameraRotationSpeed(float rotationSpeed)
    {
        _rotationSpeed = rotationSpeed;
    }

    private void RotateByRecoil()
    {
        float[][] recoils = WeaponManager.Instance.GetRecoil(3, 1);

        _recoil = new Vector3(UnityEngine.Random.Range(recoils[1][0], recoils[1][1]), UnityEngine.Random.Range(recoils[0][0], recoils[0][1]), 0);
    }
}

