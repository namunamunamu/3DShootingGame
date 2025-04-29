using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStatus PlayerData;
    public PlayerMove PlayerMove;
    public PlayerRotate PlayerRotate;
    public PlayerFire PlayerFire;
    public CharacterController CharacterController;

    public float MoveSpeed;

    private void Awake()
    {
        PlayerData = GetComponent<PlayerStatus>();
        PlayerMove = GetComponent<PlayerMove>();
        PlayerRotate = GetComponent<PlayerRotate>();
        PlayerFire = GetComponent<PlayerFire>();
        CharacterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        PlayerData.InitializeStatData();
    }

}
