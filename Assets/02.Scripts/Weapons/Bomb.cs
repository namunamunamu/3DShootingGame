using UnityEngine;

public class Bomb : MonoBehaviour
{
    // 목표: 마우스의 오른쪽 버튼을 누르면 카메라가 바라보는 방향으로 수류탄을 던지고 싶다.
    // 1. 수류탄 오브젝트 만들기
    public int BombDamage = 30;
    public float BombKnockBack = 10f;


    // 충돌했을 때 처리
    private void OnCollisionEnter(Collision collision)
    {
        ParticleSystem effectObjet = PoolManager.Instance.GetVFX("BombExplosionVFX");
        effectObjet.gameObject.transform.position = transform.position;
        effectObjet.Play();

        if(collision.gameObject.tag == "Enemy")
        {
            Damage damage = new Damage(){Value = BombDamage, KnockBackPower = BombKnockBack, From = gameObject};

            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        gameObject.SetActive(false);       
    }
}
