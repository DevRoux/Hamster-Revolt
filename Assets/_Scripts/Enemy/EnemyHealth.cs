using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    public TMP_Text enemyHealthText;
    public UnityEvent onDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
        UpdateEnemyHealthText();
    }

    public void UpdateEnemyHealthText()
    {
        if (enemyHealthText != null)
        {
            enemyHealthText.text = currentHealth.ToString();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateEnemyHealthText();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateEnemyHealthText();
    }

    private void Die()
    {
        Debug.Log("Enemy died!");

        if (onDeath != null)
        {
            onDeath.Invoke();
        }

        Destroy(gameObject);
    }
}
