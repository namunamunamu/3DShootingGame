using System.ComponentModel;
using UnityEngine;

public class EnemyAttackState : IEnemeyState
{
    private  EnemyBase _enemy;
    private GameObject _player; 
    private EnemyStateMachine _stateMachine;

    private float _attackTimer;

    public void InitializeState(EnemyStateMachine stateMachine, EnemyBase enemy, GameObject player)
    {
        _enemy = enemy;
        _player = player;
        _stateMachine = stateMachine;
    }

    public void ExcuteState()
    {
        _attackTimer += Time.deltaTime;

        if(Vector3.Distance(_enemy.transform.position, _player.transform.position) > _enemy.EnemyData.AttackDistance)
        {
            Debug.Log("상태전환: Attack -> Trace");
            _stateMachine.SetState(EEnemyState.Trace);
            _attackTimer = 0f;
            return;
        }

        _attackTimer += Time.deltaTime;
        if(_attackTimer >= _enemy.EnemyData.AttackCoolTime)
        {
            Debug.Log("공격!");
            Collider[] hitColliders = Physics.OverlapSphere(_enemy.transform.position, 2f);
            foreach(Collider collider in hitColliders)
            {
                if(collider.gameObject.CompareTag("Player"))
                {
                    collider.gameObject.GetComponent<PlayerController>().TakeDamage(_enemy.Damage);
                }
            }
            _attackTimer = 0f;
        }
    }

    public void EndState()
    {
        _attackTimer = 0f;
    }
}
