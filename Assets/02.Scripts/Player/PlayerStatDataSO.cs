using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatDataSO", menuName = "Scriptable Objects/PlayerStatDataSO")]
public class PlayerStatDataSO : ScriptableObject
{
    public float _moveSpeed;
    public float _sprintSpeed;
    public float _climbSpeed;
    public float _dashPower;
    public float _jumpPower;
    public int _maxMultiJump;

    public float _maxStamina;
    public float _staminaRecovery;
    public float _sprintStamina;
    public float _dashStamina;
    public float _dashDuration;
    public float _climbStamina;
    public float _jumpStamina;
}
