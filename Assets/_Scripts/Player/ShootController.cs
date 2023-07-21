using UnityEngine;

public class ShootController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int bulletDamage = 2;
    public float shotDelay = 1f;

    private float shotTimer;
    private CharacterController playerController;

    private void Awake()
    {
        playerController = GetComponent<CharacterController>();
    }

    public void Shoot(Vector2 direction)
    {
        if (!playerController.IsAttacking && shotTimer <= 0f) // Check if not attacking
        {
            if (direction == Vector2.zero)
            {
                direction = playerController.lastInputDirection;
            }

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.damage = bulletDamage;
            bulletScript.SetDirection(direction);

            shotTimer = shotDelay;
        }
    }

    private void Update()
    {
        if (shotTimer > 0f)
        {
            shotTimer -= Time.deltaTime;
        }
    }
}
