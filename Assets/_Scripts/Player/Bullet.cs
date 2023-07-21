using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 2;
    public float speed = 10f;
    public float lifetime = 2f;

    private Rigidbody2D rb;
    private Vector2 shootingDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.velocity = shootingDirection * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }

    public void SetDirection(Vector2 direction)
    {
        shootingDirection = direction.normalized;
    }
}
