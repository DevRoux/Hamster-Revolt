using UnityEngine;

public class EnemyBigPotion : MonoBehaviour
{
    public GameObject newEnemyPrefab;
    public float destroyDelay = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyAI enemy = other.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            ReplaceEnemy(enemy.gameObject);
        }

        gameObject.SetActive(false);
        Destroy(gameObject, destroyDelay);
    }

    private void ReplaceEnemy(GameObject enemy)
    {
        Vector3 enemyPosition = enemy.transform.position;
        Quaternion enemyRotation = enemy.transform.rotation;

        GameObject newEnemy = Instantiate(newEnemyPrefab, enemyPosition, enemyRotation);

        Destroy(enemy);
    }
}
