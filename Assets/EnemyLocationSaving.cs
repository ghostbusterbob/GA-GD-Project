using System.IO;
using UnityEngine;

[System.Serializable]
public class EnemyLocationData
{
    public SerializableVector3[] enemyPositions;
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
    public Transform[] enemyPositions;

    private string saveFilePath;

    private void Start()
    {
        LoadEnemyLocations();
    }

    private void Awake()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Transform enemyTransform = go.transform;
            if (enemyTransform != null)
            {
                if (enemyPositions == null || enemyPositions.Length < GameObject.FindGameObjectsWithTag("Enemy").Length)
                {
                    enemyPositions = new Transform[GameObject.FindGameObjectsWithTag("Enemy").Length];
                }
                enemyPositions[System.Array.IndexOf(enemyPositions, null)] = enemyTransform;
            }
        }
        enemylocationsavingsystem = this;
        saveFilePath = Path.Combine(Application.persistentDataPath, "enemylocation.txt");
        LoadEnemyLocations();
    }
    private void OnApplicationQuit()
    {
        SaveEnemyLocations();
    }
    private void SaveEnemyLocations()
    {
        EnemyLocationData data = new EnemyLocationData();
        data.enemyPositions = new SerializableVector3[enemyPositions.Length];

        for (int i = 0; i < enemyPositions.Length; i++)
        {
            if(enemyPositions != null)
            data.enemyPositions[i] = new SerializableVector3(enemyPositions[i].position);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
    }

    private void LoadEnemyLocations()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            EnemyLocationData data = JsonUtility.FromJson<EnemyLocationData>(json);

            if (data.enemyPositions != null && enemyPositions != null)
            {
                for (int i = 0; i < data.enemyPositions.Length && i < enemyPositions.Length; i++)
                {
                    enemyPositions[i].position = data.enemyPositions[i].ToVector3();
                }
            }
        }
    }
}
