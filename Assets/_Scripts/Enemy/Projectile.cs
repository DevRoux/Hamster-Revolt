using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 2;
    public float speed = 10f;
    public float lifetime = 2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Shoot(Vector2 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
