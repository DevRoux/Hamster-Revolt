using UnityEngine;

public class EnemySmallPotion : MonoBehaviour
{
    public float scalingFactor = 0.5f;
    public float duration = 4f;
    public float destroyDelay = 5f;

    private bool isScaling = false;
    private float originalScale;
    private EnemyAI targetEnemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyAI enemy = other.GetComponent<EnemyAI>();
        if (enemy != null && !isScaling)
        {
            isScaling = true;
            targetEnemy = enemy;
            originalScale = enemy.transform.localScale.x;
            ScaleEnemy(enemy.gameObject);
            gameObject.SetActive(false);
            Destroy(gameObject, destroyDelay);
        }
    }

    private void ScaleEnemy(GameObject enemy)
    {
        enemy.transform.localScale *= scalingFactor;
        Invoke(nameof(RevertScaling), duration);
    }

    private void RevertScaling()
    {
        if (isScaling && targetEnemy != null)
        {
            targetEnemy.transform.localScale = new Vector3(originalScale, originalScale, originalScale);
            isScaling = false;
            targetEnemy = null;
        }
    }
}
