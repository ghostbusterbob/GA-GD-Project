using System.IO;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EnemyLocationData
{
    public SerializableVector3[] enemyPositions;
    public int maxEnemies;
    public int wave;
    public int enemiesSpawned;
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
    [Header("Spawn Points")]
[SerializeField] private Transform[] spawnPoints;

    public static EnemyLocationSaving enemylocationsavingsystem;

    [Header("Enemy Spawning")]
    public GameObject[] enemyPrefabs;
    public int maxEnemies = 1;
    public float spawnRadius = 10f;
    public Transform centerPoint;

    [SerializeField] private Text waveText;
    [SerializeField] private Text currentenemys;

    private Transform[] enemyPositions;
    private string saveFilePath;

    [SerializeField] private int enemiesSpawned;
    [SerializeField] private int wave;

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
        currentenemys.text = "Current enemies: " + enemiesSpawned;
        waveText.text = "Current wave: " + wave;
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

    public void changeWave(int wavecount)
    {
        wave = wavecount;
    }

    private void SaveEnemyLocations()
    {
        if (enemyPositions == null || enemyPositions.Length == 0)
            return;

        EnemyLocationData data = new EnemyLocationData
        {
            wave = wave,
            enemiesSpawned = enemiesSpawned,
            maxEnemies = maxEnemies,
            enemyPositions = new SerializableVector3[enemiesSpawned]
        };

        int index = 0;
        foreach (var pos in enemyPositions)
        {
            if (pos != null)
            {
                data.enemyPositions[index] = new SerializableVector3(pos.position);
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
                    wave = data.wave;
                    enemiesSpawned = 0;
                    maxEnemies = data.maxEnemies;
                    enemyPositions = new Transform[maxEnemies];

                    int loadedCount = data.enemyPositions.Length;
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

        wave = 1;
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
    // Pick a spawn point using modulo to loop if more enemies than spawn points
    Transform spawnPoint = spawnPoints[index % spawnPoints.Length];
    Vector3 spawnPosition = spawnPoint.position;

    GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
    GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    enemy.tag = "Enemy";

    if (enemyPositions.Length <= index)
    {
        System.Array.Resize(ref enemyPositions, maxEnemies);
    }

    enemyPositions[index] = enemy.transform;
    enemiesSpawned++;

    Debug.Log($"Spawned enemy at {spawnPosition}");
}


    public void CheckWave()
    {
        if (enemiesSpawned <= 0 && !hasCheckedWave)
        {
            hasCheckedWave = true;
            wave++;
            maxEnemies++;
            //enemiesSpawned++;
            respawnenemys();
        }
    }

    public void killEnemy()
    {
        if (enemiesSpawned > 0)
        {
            enemiesSpawned--;
            Debug.Log("Enemy killed, enemies remaining: " + enemiesSpawned);
            CheckWave();
        }
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

        hasCheckedWave = false;
    }
}
