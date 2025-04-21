using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 목표 : WASD를 누르면 캐릭터를 이동시키고 싶다.
    // 필요 속성:
    // - 이동속도
    private CharacterController _characterController;

    private const float GRAVITY = -9.8f;
    private float _yVelocity = 0f;

    private int _jumpChance;
    private bool _isDash = false;
    private float _dashTimer=0f;


    // 구현순서:
    // 1. 키모드 입력을 받느다.
    // 2. 입력으로부터 방향을 설정한다.
    // 3. 방향에 따라 플레이어를 이동한다.

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _jumpChance = PlayerStatus.Instance.MaxMultiJump;
    }

    void Update()
    {
        CheckDash();
        Move();
        Jump();
    }

    private void Jump()
    {
        
        if(_characterController.isGrounded)
        {
            _jumpChance = PlayerStatus.Instance.MaxMultiJump;
        }

        if(_characterController.collisionFlags == CollisionFlags.Sides)
        {
            if(_jumpChance != PlayerStatus.Instance.MaxMultiJump)
            {
                ++_jumpChance;
            }
        }

        if(Input.GetKeyDown(KeyCode.Space) && PlayerStatus.Instance.Stamina >= PlayerStatus.Instance.JumpStamina && _jumpChance > 0)
        {
            --_jumpChance;
            _yVelocity = PlayerStatus.Instance.JumpPower;
            PlayerStatus.Instance.SetStamina(-PlayerStatus.Instance.JumpStamina);
        }
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;
        // 메인 카메라를 기준으로 방향을 반환한다.
        dir = Camera.main.transform.TransformDirection(dir);

        if(_isDash)
        {
            dir *= PlayerStatus.Instance.DashPower;
        }

        // 중력 적용
        _yVelocity += GRAVITY * Time.deltaTime;
        dir.y = _yVelocity;

        if(!Input.GetKey(KeyCode.LeftShift))
        {
            PlayerStatus.Instance.SetStamina(PlayerStatus.Instance.StaminaRecovery * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.LeftShift) && PlayerStatus.Instance.Stamina > 0)
        {
            Climbing();

            Sprinting(dir);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(!_isDash)
            {
                _isDash = true;
                PlayerStatus.Instance.SetStamina(-PlayerStatus.Instance.DashStamina);
            }
        }

        _characterController.Move(dir * PlayerStatus.Instance.MoveSpeed * Time.deltaTime);
    }

    private void Sprinting(Vector3 direction)
    {
        _characterController.Move(direction * PlayerStatus.Instance.SprintSpeed * Time.deltaTime);
        PlayerStatus.Instance.SetStamina(-PlayerStatus.Instance.SprintStamina * Time.deltaTime);
    }

    private void Climbing()
    {
        if(_characterController.collisionFlags == CollisionFlags.Sides)
        {
            _yVelocity = PlayerStatus.Instance.ClimbSpeed * Time.deltaTime;
            PlayerStatus.Instance.SetStamina(-PlayerStatus.Instance.ClimbStamina * Time.deltaTime);
        }
    }

    private void CheckDash()
    {
        if(_isDash)
        {
            _dashTimer += Time.deltaTime;
            if(_dashTimer >= PlayerStatus.Instance.DashDuration) 
            {
                _isDash = false;
                _dashTimer = 0f;
            }
        }
    }
}
