using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject[] spawnPoints;
    public float spawnTime = 2f;
    public float spawnDelay = 1f;
    public int maxEnemies = 5;
    public int currentEnemies = 0;
    public EnemyLocationSaving enemyLocationSaving;

    private int wave;

    public void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnDelay, spawnTime);
    }

    //
    public void enemykilled()
{
    currentEnemies = Mathf.Max(0, currentEnemies - 1);
    enemyLocationSaving.killEnemy();
    Debug.Log("killed enemy");
}

    void Update()
    {
       
    }
}