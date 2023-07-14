using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    // Custom variables
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float attackDistance = 2f;
    public int minDamage = 2;
    public int maxDamage = 4;
    public float attackDelay = 0.5f;
    public float pushbackForce = 2f;

    private Rigidbody2D rb;
    private bool isAttacking;
    private float attackTimer;
    private bool isInvulnerable;
    private float invulnerabilityTime;
    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;
    public Sprite damagedSprite;
    public Sprite attackingSprite;
    public float spriteChangeDuration = 0.5f;

    // Health system
    public int maxHealth = 10;
    public int currentHealth;
    public TMP_Text playerHealthText;

    // Movement system
    private string horizontalAxis = "Horizontal";
    private string verticalAxis = "Vertical";

    // Attack system
    private string attackButton = "Attack";

    // Ranged attack system
    public GameObject bulletPrefab;
    public int bulletDamage = 2;
    public float shotDelay = 1f;
    private float shotTimer;

    // Audio system
    public AudioClip damageSound;
    public AudioClip attackSound;
    public AudioClip fireSound;
    public float soundVolume = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdatePlayerHealthText();
        attackTimer = 0f;
        shotTimer = 0f;
        invulnerabilityTime = 0f;
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw(horizontalAxis);
        float moveY = Input.GetAxisRaw(verticalAxis);

        Vector2 movement = new Vector2(moveX, moveY).normalized * moveSpeed;

        rb.velocity = movement;
        //RotateTowardsMouse();

        if (Input.GetButtonDown(attackButton) && !isAttacking && attackTimer <= 0f)
        {
            StartCoroutine(PerformAttack());
            attackTimer = attackDelay;
        }

        if (Input.GetButtonDown("Fire") && !isAttacking && shotTimer <= 0f)
        {
            Shoot();
            shotTimer = shotDelay;
        }

        if (shotTimer > 0f)
        {
            shotTimer -= Time.deltaTime;
        }

        if (isInvulnerable)
        {
            invulnerabilityTime -= Time.deltaTime;

            if (invulnerabilityTime <= 0)
            {
                isInvulnerable = false;
                spriteRenderer.sprite = originalSprite;
            }
        }

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void RotateTowardsMouse()
    {
        // Look at the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    private System.Collections.IEnumerator PerformAttack()
    {
        isAttacking = true;

        Vector2 attackDirection = transform.right;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, attackDirection, attackDistance);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    int damage = Random.Range(minDamage, maxDamage + 1);
                    enemyHealth.TakeDamage(damage);

                    // Push back the enemy
                    PushEnemyBack(hit.collider.gameObject, attackDirection);
                }
            }
        }

        StartCoroutine(ChangeSpriteForDuration(attackingSprite, spriteChangeDuration));

        PlaySound(attackSound);

        isAttacking = false;

        yield return null;
    }

    private void PushEnemyBack(GameObject enemy, Vector2 attackDirection)
    {
        Rigidbody2D enemyRB = enemy.GetComponent<Rigidbody2D>();
        if (enemyRB != null)
        {
            Vector2 pushbackDirection = -attackDirection.normalized;

            enemyRB.AddForce(pushbackDirection * pushbackForce, ForceMode2D.Impulse);
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage = bulletDamage;

        PlaySound(fireSound);
    }

    public void TakeDamage(int damage)
    {
        if (!isInvulnerable)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                isInvulnerable = true;
                invulnerabilityTime = 2f;
                StartCoroutine(ChangeSpriteForDuration(damagedSprite, spriteChangeDuration));

                PlaySound(damageSound);
            }
        }
        UpdatePlayerHealthText();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdatePlayerHealthText();
    }

    private System.Collections.IEnumerator ChangeSpriteForDuration(Sprite newSprite, float duration)
    {
        spriteRenderer.sprite = newSprite;

        yield return new WaitForSeconds(duration);

        spriteRenderer.sprite = originalSprite;
    }

    private void Die()
    {
        SceneManager.LoadScene("GameOver");
        Debug.Log("Game Over");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    private void UpdatePlayerHealthText()
    {
        if (playerHealthText != null)
        {
            playerHealthText.text = currentHealth.ToString();
        }
    }

    private void PlaySound(AudioClip soundClip)
    {
        AudioSource.PlayClipAtPoint(soundClip, transform.position, soundVolume);
    }
}
