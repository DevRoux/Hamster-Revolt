using UnityEngine;

public enum EnemyState
{
    Idle,
    Chase,
    Attack
}

public class EnemyAI : MonoBehaviour
{
    public float detectionRange = 5f;
    public float attackRange = 1f;
    public float moveSpeed = 3f;
    public int damage = 1;
    public float freezeTime = 1f;
    public float pushbackForce = 2f;

    private EnemyState currentState = EnemyState.Idle;
    private GameObject player;
    private bool isFrozen;
    private Rigidbody2D rb;
    private Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isFrozen)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            switch (currentState)
            {
                case EnemyState.Idle:
                    if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
                    {
                        currentState = EnemyState.Chase;
                    }
                    break;

                case EnemyState.Chase:
                    if (distanceToPlayer > detectionRange)
                    {
                        currentState = EnemyState.Idle;
                    }
                    else if (distanceToPlayer <= attackRange)
                    {
                        currentState = EnemyState.Attack;
                    }
                    else
                    {
                        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
                        MoveInFourDirections(directionToPlayer);
                    }
                    break;

                case EnemyState.Attack:
                    StartCoroutine(AttackPlayer());
                    currentState = EnemyState.Idle;
                    break;
            }
        }
    }

    private void MoveInFourDirections(Vector2 targetDirection)
    {
        float maxDot = -Mathf.Infinity;
        int closestDirectionIndex = 0;

        for (int i = 0; i < directions.Length; i++)
        {
            float dot = Vector2.Dot(targetDirection, directions[i]);
            if (dot > maxDot)
            {
                maxDot = dot;
                closestDirectionIndex = i;
            }
        }

        Vector2 chosenDirection = directions[closestDirectionIndex];
        rb.MovePosition(rb.position + chosenDirection * moveSpeed * Time.deltaTime);
    }

    public void Freeze(float duration)
    {
        isFrozen = true;
        StartCoroutine(UnfreezeAfterDelay(duration));
    }

    private System.Collections.IEnumerator UnfreezeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isFrozen = false;
    }

    private System.Collections.IEnumerator AttackPlayer()
    {
        isFrozen = true;
        player.GetComponent<CharacterController>().TakeDamage(damage);
        yield return new WaitForSeconds(freezeTime);
        isFrozen = false;
    }

    public void TakeDamage(int damage, Vector2 attackDirection)
    {
        PushEnemyBack(attackDirection);
    }

    private void PushEnemyBack(Vector2 attackDirection)
    {
        Vector2 pushbackDirection = -attackDirection.normalized;
        rb.AddForce(pushbackDirection * pushbackForce, ForceMode2D.Impulse);
    }
}
