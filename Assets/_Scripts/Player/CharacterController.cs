using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float attackDistance = 2f;
    public int minDamage = 2;
    public int maxDamage = 4;
    public float attackDelay = 0.5f;
    public float pushbackForce = 2f;
    public int maxHealth = 10;
    public TMP_Text playerHealthText;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isAttacking;
    private float attackTimer;
    private bool isInvulnerable;
    private float invulnerabilityTime;

    private int currentHealth;

    public Vector2 lastInputDirection = Vector2.right;
    private string horizontalAxis = "Horizontal";
    private string verticalAxis = "Vertical";
    private string attackButton = "Attack";

    private ShootController shootController;
    public bool IsAttacking => isAttacking;

    public AudioClip damageSound;
    public AudioClip attackSound;
    public AudioClip fireSound;
    public float soundVolume = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        shootController = GetComponent<ShootController>();
    }

    private void Start()
    {
        rb.freezeRotation = true;
        currentHealth = maxHealth;
        UpdatePlayerHealthText();
        attackTimer = 0f;
        invulnerabilityTime = 0f;
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw(horizontalAxis);
        float moveY = Input.GetAxisRaw(verticalAxis);

        if (moveX != 0f || moveY != 0f)
        {
            lastInputDirection = new Vector2(moveX, moveY).normalized;
            rb.velocity = lastInputDirection * moveSpeed;
            animator.SetFloat("MoveX", lastInputDirection.x);
            animator.SetFloat("MoveY", lastInputDirection.y);
        }
        else
        {
            rb.velocity = Vector2.zero; // Set velocity to zero when no input
        }

        if (Input.GetButtonDown(attackButton) && !isAttacking && attackTimer <= 0f)
        {
            StartCoroutine(PerformAttack());
            attackTimer = attackDelay;
        }

        if (Input.GetButtonDown("Fire") && !isAttacking)
        {
            shootController.Shoot(lastInputDirection);
        }

        if (isInvulnerable)
        {
            invulnerabilityTime -= Time.deltaTime;

            if (invulnerabilityTime <= 0)
            {
                isInvulnerable = false;
                animator.SetBool("Damaged", false);
            }
        }

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private System.Collections.IEnumerator PerformAttack()
    {
        isAttacking = true;

        Vector2 attackDirection = lastInputDirection;
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
                    PushEnemyBack(hit.collider.gameObject, attackDirection);
                }
            }
        }

        PlaySound(attackSound);

        yield return new WaitForSeconds(attackDelay);

        isAttacking = false; // Reset the isAttacking flag

        // Rest of the logic
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

    public void TakeDamage(int damage)
    {
        if (!isInvulnerable)
        {
            animator.SetBool("Damaged", true);
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                isInvulnerable = true;
                invulnerabilityTime = 2f;
            }
        }
        UpdatePlayerHealthText();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdatePlayerHealthText();
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
