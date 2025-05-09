using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private PlayerController _playerController;

    private PlayerStatus _playerData;

    private const float GRAVITY = -9.8f;
    private float _yVelocity = 0f;
    private int _jumpChance;
    private bool _isDash = false;
    private float _dashTimer=0f;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _jumpChance = _playerController.PlayerData.MaxMultiJump;
        _playerData = _playerController.PlayerData;
    }

    void Update()
    {
        CheckDash();
        Move();
        Jump();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        if(dir.sqrMagnitude != 0 )
        {
            _playerController.PlayerAnimator.SetBool("isMoving", true);
        }
        else
        {
            _playerController.PlayerAnimator.SetBool("isMoving", false);
        }

        dir = dir.normalized;
        dir = Camera.main.transform.TransformDirection(dir);

        if(_isDash)
        {
            dir *= _playerController.PlayerData.DashPower;
        }

        // 중력 적용
        _yVelocity += GRAVITY * Time.deltaTime;
        dir.y = _yVelocity;

        if(!Input.GetKey(KeyCode.LeftShift))
        {
           _playerController.PlayerData.SetStamina(_playerController.PlayerData.StaminaRecovery * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.LeftShift) && _playerController.PlayerData.Stamina > 0)
        {
            Climbing();

            Sprinting(dir);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(!_isDash)
            {
                _isDash = true;
                _playerController.PlayerData.SetStamina(-_playerController.PlayerData.DashStamina);
            }
        }

        _playerController.CharacterController.Move(dir * _playerController.PlayerData.MoveSpeed * Time.deltaTime);
    }
    
    private void Jump()
    {
        
        if(_playerController.CharacterController.isGrounded)
        {
            _jumpChance = _playerController.PlayerData.MaxMultiJump;
        }

        if(_playerController.CharacterController.collisionFlags == CollisionFlags.Sides)
        {
            if(_jumpChance != _playerController.PlayerData.MaxMultiJump)
            {
                ++_jumpChance;
            }
        }

        if(Input.GetKeyDown(KeyCode.Space) && _playerController.PlayerData.Stamina >= _playerController.PlayerData.JumpStamina && _jumpChance > 0)
        {
            --_jumpChance;
            _yVelocity = _playerController.PlayerData.JumpPower;
            _playerController.PlayerData.SetStamina(-_playerController.PlayerData.JumpStamina);
        }


    }


    private void Sprinting(Vector3 direction)
    {
        _playerController.CharacterController.Move(direction * _playerController.PlayerData.SprintSpeed * Time.deltaTime);
        _playerController.PlayerData.SetStamina(-_playerController.PlayerData.SprintStamina * Time.deltaTime);
    }

    private void Climbing()
    {
        if(_playerController.CharacterController.collisionFlags == CollisionFlags.Sides)
        {
            _yVelocity = _playerController.PlayerData.ClimbSpeed * Time.deltaTime;
            _playerController.PlayerData.SetStamina(-_playerController.PlayerData.ClimbStamina * Time.deltaTime);
        }
    }

    private void CheckDash()
    {
        if(_isDash)
        {
            _dashTimer += Time.deltaTime;
            if(_dashTimer >= _playerController.PlayerData.DashDuration) 
            {
                _isDash = false;
                _dashTimer = 0f;
            }
        }
    }
}

