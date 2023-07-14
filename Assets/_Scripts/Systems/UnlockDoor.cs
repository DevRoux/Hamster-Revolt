using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    public GameObject doorToUnlock;
    public HamsterSpawner enemySpawner;
    public int enemySpawnCountThreshold = 10;
    public float unlockDelay = 10f;
    private bool isDoorUnlocked;

    private void Start()
    {
        enemySpawner = FindObjectOfType<HamsterSpawner>();
        if (enemySpawner == null)
        {
            Debug.LogError("No SpawnMonster script found in the scene.");
            Destroy(gameObject);
        }
        else
        {
            enemySpawner.onEnemySpawned += CheckUnlockCondition;
        }
    }

    private void OnDestroy()
    {
        if (enemySpawner != null)
        {
            enemySpawner.onEnemySpawned -= CheckUnlockCondition;
        }
    }

    private void CheckUnlockCondition()
    {
        if (!isDoorUnlocked && enemySpawner.GetSpawnedEnemyCount() >= enemySpawnCountThreshold)
        {
            isDoorUnlocked = true;
            Invoke("UnlockTheDoor", unlockDelay);
        }
    }

    private void UnlockTheDoor()
    {
        if (doorToUnlock != null)
        {
            doorToUnlock.SetActive(false);
        }
    }
}
