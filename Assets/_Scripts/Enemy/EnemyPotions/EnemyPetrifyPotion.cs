using UnityEngine;

public class EnemyPetrifyPotion : MonoBehaviour
{
    public float freezeDuration = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyAI enemy = other.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            enemy.Freeze(freezeDuration);
        }
        Destroy(gameObject);
    }
}
