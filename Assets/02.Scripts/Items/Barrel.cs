using System.Collections;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField]
    private int _health;

    [SerializeField]
    private int _damageValue;
    [SerializeField]
    private int _impact;
    [SerializeField]
    private float _radius;

    [SerializeField]
    private float _destroyTime;

    private Rigidbody _rigidBody;

    private Damage damage = new Damage();

    private void Awake()
    {
        damage.Value = _damageValue;
        damage.KnockBackPower = _impact;
        damage.From = gameObject;
        _rigidBody = GetComponent<Rigidbody>();
        
    }

    public void Initialize()
    {
    }

    public void TakeDamage(Damage damage)
    {
        Vector3 impactDirection = (transform.position - damage.From.transform.position).normalized;
        _rigidBody.AddForce(impactDirection * damage.KnockBackPower,ForceMode.Impulse);

        _health -= damage.Value;
        if(_health <= 0)
        {
            Explod();
        }
    }

    private void Explod()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _radius);
        foreach(Collider collider in hitColliders)
        {
            if(collider.gameObject.CompareTag("Enemy"))
            {
                collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }
        }

        _rigidBody.AddForce(Vector3.up * damage.KnockBackPower, ForceMode.Impulse);
        StartCoroutine(DestroyTimer(_destroyTime));
    }

    private IEnumerator DestroyTimer(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
