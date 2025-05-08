using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStatus PlayerData;
    public PlayerMove PlayerMove;
    public PlayerRotate PlayerRotate;
    public PlayerFire PlayerFire;
    public CharacterController CharacterController;

    public float MoveSpeed;

    public Action<Damage> OnAttacked;

    private void Awake()
    {
        PlayerData = GetComponent<PlayerStatus>();
        PlayerMove = GetComponent<PlayerMove>();
        PlayerRotate = GetComponent<PlayerRotate>();
        PlayerFire = GetComponent<PlayerFire>();
        CharacterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        PlayerData.InitializeStatData();
    }

    public void TakeDamage(Damage damage)
    {
        OnAttacked?.Invoke(damage);
        PlayerData.SetHealth(-damage.Value);
        

        if(PlayerData.Health <= 0)
        {
            Destroy(gameObject);
        }
    }

}
