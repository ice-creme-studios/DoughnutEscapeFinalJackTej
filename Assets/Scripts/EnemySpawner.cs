using UnityEngine;

public class ProgressiveEnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;    // Enemy prefab to spawn
    public float spawnRadius = 0.1f;    // Radius around the spawner

    [Header("Timing Settings")]
    public float spawnInterval = 5f;  // Initial seconds between spawns
    public float duration = 180f;     // Total duration in seconds (3 minutes)
    private float elapsedTime = 0f;

    private int spawnCount = 1;       // Number of enemies to spawn each interval

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= duration)
        {
            // Stop spawning after 3 minutes
            enabled = false;
            return;
        }

        // Gradually increase spawn rate or number of enemies
        // Example: every 30 seconds, increase spawnCount by 1
        spawnCount = 1 + Mathf.FloorToInt(elapsedTime / 30f);
    }

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemies), 0f, spawnInterval);
    }

    void SpawnEnemies()
    {
        if (enemyPrefab == null) return;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 randomPos = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = transform.position + new Vector3(randomPos.x, 0f, randomPos.y);

            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }
}
