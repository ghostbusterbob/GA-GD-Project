using System.IO;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EnemyLocationData
{
    public SerializableVector3[] enemyPositions;
    public int wave;
}

[System.Serializable]
public struct SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public Vector3 ToVector3() => new Vector3(x, y, z);
}

public class EnemyLocationSaving : MonoBehaviour
{
    public static EnemyLocationSaving enemylocationsavingsystem;

    [Header("Enemy Spawning")]
    public GameObject[] enemyPrefabs;
    public int maxEnemies = 1;
    public float spawnRadius = 10f;
    public Transform centerPoint;

    [SerializeField] Text waveText;

    private Transform[] enemyPositions;
    private string saveFilePath;

    private int enemiesSpawned;
    private int wave;



    private bool hasCheckedWave = false;

    private void Awake()
    {
        enemylocationsavingsystem = this;
        saveFilePath = Path.Combine(Application.persistentDataPath, "enemylocation.json");

    }

    private void Start()
    {
        LoadEnemyLocationsOrSpawn();
        Debug.Log($"Save file path: {saveFilePath}");
    }

    private void Update()
    {
        Debug.Log($"Current enemies spawned: {enemiesSpawned}");

    }

    private void OnApplicationQuit()
    {
        SaveEnemyLocations();
    }

    public void savingdata1()
    {
        SaveEnemyLocations();
    }

    private void SaveEnemyLocations()
    {
        if (enemyPositions == null || enemyPositions.Length == 0)
            return;

        EnemyLocationData data = new EnemyLocationData();
        data.wave = wave;
        data.enemyPositions = new SerializableVector3[enemiesSpawned];

        int index = 0;
        for (int i = 0; i < enemyPositions.Length; i++)
        {
            if (enemyPositions[i] != null)
            {
                data.wave = wave;
                data.enemyPositions[index] = new SerializableVector3(enemyPositions[i].position);
                index++;
            }
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);

        Debug.Log($"Enemy positions saved to {saveFilePath}");
    }

    public void LoadEnemyLocationsOrSpawn()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(saveFilePath);
                EnemyLocationData data = JsonUtility.FromJson<EnemyLocationData>(json);

                if (data != null && data.enemyPositions != null && data.enemyPositions.Length > 0)
                {
                    int loadedCount = data.enemyPositions.Length;
                    wave = data.wave;
                    CheckWave();
                    waveText.text = "Current wave " + wave.ToString();
                    enemyPositions = new Transform[maxEnemies];
                    enemiesSpawned = 0;

                    for (int i = 0; i < loadedCount && i < maxEnemies; i++)
                    {
                        Vector3 pos = data.enemyPositions[i].ToVector3();
                        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                        GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
                        enemy.tag = "Enemy";
                        enemyPositions[i] = enemy.transform;
                        enemiesSpawned++;

                        Debug.Log($"Loaded enemy at {pos}");
                    }

                    for (int i = loadedCount; i < maxEnemies; i++)
                    {
                        SpawnEnemyAtIndex(i);
                    }

                    return;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Failed to load enemy positions: " + ex.Message);
            }
        }

        SpawnEnemiesRandomly();
    }

    private void SpawnEnemiesRandomly()
    {
        enemyPositions = new Transform[maxEnemies];
        enemiesSpawned = 0;

        for (int i = 0; i < maxEnemies; i++)
        {
            SpawnEnemyAtIndex(i);
        }
    }

    private void SpawnEnemyAtIndex(int index)
    {
        Vector3 randomPos = centerPoint.position + Random.insideUnitSphere * spawnRadius;
        randomPos.y = centerPoint.position.y;

        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject enemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);
        enemy.tag = "Enemy";

        if (enemyPositions.Length <= index)
        {
            System.Array.Resize(ref enemyPositions, maxEnemies);
        }

        enemyPositions[index] = enemy.transform;
        enemiesSpawned++;

        Debug.Log($"Spawned enemy at {randomPos}");
    }

    public void CheckWave()
    {
        if (enemiesSpawned <= 0 && !hasCheckedWave)
        {
            hasCheckedWave = true;  // Prevent re-entry while respawning
            wave++;
            maxEnemies++;
            Debug.Log($"Wave {wave} starting. Increasing maxEnemies to {maxEnemies}");
            respawnenemys();
            waveText.text = "Current wave " + wave.ToString();
        }
    }


    public void killEnemy()
{
    enemiesSpawned = Mathf.Max(0, enemiesSpawned - 1);  // ✅ Safe decrement
    CheckWave();
}


    public void respawnenemys()
    {
        if (enemyPositions == null || enemyPositions.Length < maxEnemies)
        {
            System.Array.Resize(ref enemyPositions, maxEnemies);
        }

        int currentCount = 0;
        for (int i = 0; i < enemyPositions.Length; i++)
        {
            if (enemyPositions[i] != null)
                currentCount++;
        }

        int enemiesToSpawn = maxEnemies - currentCount;
        int spawned = 0;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            for (int j = 0; j < enemyPositions.Length; j++)
            {
                if (enemyPositions[j] == null)
                {
                    SpawnEnemyAtIndex(j);
                    spawned++;
                    break;
                }
            }
        }

        enemiesSpawned += spawned;  // ✅ Correctly increment by how many were actually spawned
        hasCheckedWave = false;     // ✅ Reset the wave checker for the next round
    }

}
