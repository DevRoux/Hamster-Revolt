using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterSpawner : MonoBehaviour
{
    public GameObject[] hamsters;
    public Transform[] spawnPoints;
    public float minSpawnInterval = 10f;
    public float maxSpawnInterval = 15f;
    public int maxEnemies = 10;

    private int enemyCount;

    public delegate void EnemySpawnedDelegate();
    public event EnemySpawnedDelegate onEnemySpawned;

    private void Start()
    {
        StartCoroutine(SpawnObjectsCoroutine());
    }

    private IEnumerator SpawnObjectsCoroutine()
    {
        while (enemyCount < maxEnemies)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject objectToSpawn = hamsters[Random.Range(0, hamsters.Length)];
            GameObject spawnedObject = Instantiate(objectToSpawn, randomSpawnPoint.position, Quaternion.identity);

            enemyCount++;
            if (onEnemySpawned != null)
            {
                onEnemySpawned.Invoke();
            }
        }
    }

    public int GetSpawnedEnemyCount()
    {
        return enemyCount;
    }
}
