using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    public TMP_Text enemyHealthText;
    public UnityEvent onDeath;
    public GameObject BossDeathWinTrigger;

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
        Destroy(gameObject);

        if (onDeath != null)
        {
            onDeath.Invoke();
        }
        if (BossDeathWinTrigger.activeSelf)
        {
            SceneManager.LoadScene("GameOverWin");
        }
    }
}
