using UnityEngine;

public class EnemyHealingPotion : MonoBehaviour
{
    public int healingAmount = 2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            // Heal the enemy
            enemy.Heal(healingAmount);
        }

        Destroy(gameObject);
    }
}
