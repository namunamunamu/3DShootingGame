using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float MoveSpeed => _moveSpeed;
    public float SprintSpeed => _sprintSpeed;
    public float ClimbSpeed => _climbSpeed;
    public float DashPower => _dashPower;
    public float JumpPower => _jumpPower;
    public int MaxMultiJump => _maxMultiJump;

    public float Health => _health;
 
    public float Stamina => _stamina;
    public float MaxStamina => _maxStamina;
    public float StaminaRecovery => _staminaRecovery;
    public float SprintStamina => _sprintStamina;
    public float DashStamina => _dashStamina;
    public float DashDuration => _dashDuration;
    public float ClimbStamina => _climbStamina;
    public float JumpStamina => _jumpStamina;

    [Header("SO 데이터")]
    [SerializeField]
    private PlayerStatDataSO _statData;

    [Header("속도 관련")]
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _sprintSpeed;
    [SerializeField]
    private float _climbSpeed;
    [SerializeField]
    private float _dashPower;
    [SerializeField]
    private float _jumpPower;
    [SerializeField]
    private int _maxMultiJump;

    [Header("체력 관련")]
    [SerializeField]
    private float _health;
    [SerializeField]
    private float _maxHealth;

    [Header("스테미나 관련")]
    [SerializeField]
    private float _stamina;
    [SerializeField]
    private float _maxStamina;
    [SerializeField]
    private float _staminaRecovery;
    [SerializeField]
    private float _sprintStamina;
    [SerializeField]
    private float _dashStamina;
    [SerializeField]
    private float _dashDuration;
    [SerializeField]
    private float _climbStamina;
    [SerializeField]
    private float _jumpStamina;

    public void InitializeStatData()
    {
        _moveSpeed = _statData._moveSpeed;
        _sprintSpeed = _statData._sprintSpeed;
        _climbSpeed = _statData._climbSpeed;
        _dashPower = _statData._dashPower;
        _jumpPower = _statData._jumpPower;
        _maxMultiJump = _statData._maxMultiJump;
        _maxHealth = _statData._maxHealth;
        _maxStamina = _statData._maxStamina;
        _staminaRecovery = _statData._staminaRecovery;
        _sprintStamina = _statData._sprintStamina;
        _dashStamina = _statData._dashStamina;
        _dashDuration = _statData._dashDuration;
        _climbStamina = _statData._climbStamina;
        _jumpStamina = _statData._jumpStamina;

        SetHealth(_maxHealth);
        SetStamina(MaxStamina);
    }

    public void SetHealth(float value)
    {
        _health += value;
        if(_health <= 0)
        {
            _health = 0;
        }

        if(_health >= _maxHealth)
        {
            _health = _maxHealth;
        }
        UI_Manager.Instance.PlayerStatusPanel.OnChangeStatus?.Invoke();
    }

    public void SetStamina(float value)
    {
        _stamina += value;
        if(_stamina <= 0)
        {
            _stamina = 0;
        }

        if(_stamina >= MaxStamina)
        {
            _stamina = MaxStamina;
        }

        UI_Manager.Instance.PlayerStatusPanel.OnChangeStatus?.Invoke();
    }
}
