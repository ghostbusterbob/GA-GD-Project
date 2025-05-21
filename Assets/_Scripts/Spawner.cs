using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject[] spawnPoints;
    public float spawnTime = 2f;
    public float spawnDelay = 1f;
    public int maxEnemies = 5;
    public int currentEnemies = 0;

    public void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnDelay, spawnTime);
    }

    void SpawnEnemy()
    {
        // Do not spawn if max enemies reached or arrays are empty
        if (currentEnemies >= maxEnemies || enemies.Length == 0 || spawnPoints.Length == 0)
            return;

        // Pick a random enemy and spawn point
        GameObject enemyToSpawn = enemies[Random.Range(0, enemies.Length)];
        GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instantiate the enemy
        Instantiate(enemyToSpawn, spawnPoint.transform.position, spawnPoint.transform.rotation);
        currentEnemies++;
    }
    public void enemykilled()
    {
        currentEnemies =- 1;
    }
}