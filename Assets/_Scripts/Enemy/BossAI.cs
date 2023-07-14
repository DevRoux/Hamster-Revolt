using UnityEngine;

public class BossAI : MonoBehaviour
{
    public Transform[] movePoints;
    public GameObject projectilePrefab;
    public Transform throwPosition;
    public float throwForce = 10f;
    public float attackIntervalMin = 5f;
    public float attackIntervalMax = 8f;
    public float attackRange = 5f;
    public float moveSpeed = 5f;
    public int damage = 8;

    public AudioClip attackSound;
    public float soundVolume = .3f;

    private int currentPointIndex = 0;
    private Transform currentTarget;
    private GameObject player;
    private bool isFrozen;
    private Rigidbody2D rb;
    private bool isAttacking;
    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;
    public Sprite attackingSprite;
    public float spriteChangeDuration = 0.5f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;

        if (movePoints.Length > 0)
        {
            currentTarget = movePoints[currentPointIndex];
        }
    }

    private void Update()
    {
        if (!isFrozen)
        {
            MoveToTarget();

            if (!isAttacking)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

                if (distanceToPlayer <= attackRange)
                {
                    isAttacking = true;
                    ShootProjectile();

                    float attackInterval = Random.Range(attackIntervalMin, attackIntervalMax);
                    Invoke(nameof(ResetAttack), attackInterval);

                    StartCoroutine(ChangeSpriteForDuration(attackingSprite, spriteChangeDuration));
                    PlaySound(attackSound);
                }
            }
        }
    }

    private void MoveToTarget()
    {
        if (currentTarget != null)
        {
            Vector2 direction = (currentTarget.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
            {
                SetNextTarget();
            }
        }
    }

    private void SetNextTarget()
    {
        currentPointIndex = (currentPointIndex + 1) % movePoints.Length;
        currentTarget = movePoints[currentPointIndex];
    }

    private void ShootProjectile()
    {
        Vector2 throwDirection = (player.transform.position - throwPosition.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, throwPosition.position, Quaternion.identity);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.damage = damage;
        projectileScript.Shoot(throwDirection, throwForce);
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    private System.Collections.IEnumerator ChangeSpriteForDuration(Sprite newSprite, float duration)
    {
        spriteRenderer.sprite = newSprite;

        yield return new WaitForSeconds(duration);

        spriteRenderer.sprite = originalSprite;
    }

    private void PlaySound(AudioClip soundClip)
    {
        AudioSource.PlayClipAtPoint(soundClip, transform.position, soundVolume);
    }

    // Other methods...
}
