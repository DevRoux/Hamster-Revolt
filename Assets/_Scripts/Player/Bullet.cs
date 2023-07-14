using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 2;
    public float speed = 10f;
    public float lifetime = 2f;
    public float shotDelay = 1f;

    private Rigidbody2D rb;
    private bool canDamage = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.velocity = transform.right * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canDamage && other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                canDamage = false;
                Destroy(gameObject, shotDelay);
            }
        }
    }
}
