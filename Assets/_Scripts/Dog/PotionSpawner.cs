using UnityEngine;

public class PotionSpawner : MonoBehaviour
{
    public GameObject[] potions;
    public float minSpawnInterval = 10f;
    public float maxSpawnInterval = 15f;
    public float spawnHeight = 5f; 
    public float throwSpeed = 5f; 

    private void Start()
    {
        StartCoroutine(SpawnObjectsCoroutine());
    }

    private System.Collections.IEnumerator SpawnObjectsCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

            // Spawn a random potion
            GameObject objectToSpawn = potions[Random.Range(0, potions.Length)];
            GameObject spawnedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);

            Vector3 throwDirection = new Vector3(Random.Range(-1f, 1f), -1f, 0f).normalized;

            Rigidbody2D rigidbody = spawnedObject.GetComponent<Rigidbody2D>();
            if (rigidbody != null)
            {
                rigidbody.AddForce(throwDirection * throwSpeed, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(Random.Range(10f, 15f));

            Destroy(spawnedObject);
        }
    }
}

