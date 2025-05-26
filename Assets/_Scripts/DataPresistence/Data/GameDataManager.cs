using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public Vector3 position;
}
public class GameDataManager : MonoBehaviour
{
    public Transform playerTransform;
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    private void Start()
    {
        LoadGame();
    }

    public void SaveGame()
    {
        PlayerData playerData = new PlayerData
        {
            position = playerTransform.position
        };
        string json = JsonUtility.ToJson(playerData);
        string path = Application.persistentDataPath + "/playerData.json";
        File.WriteAllText(path, json);
    }

    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/playerData.json";
        string json = File.ReadAllText(path);
        PlayerData loadedData = JsonUtility.FromJson<PlayerData>(json);
        playerTransform.position = loadedData.position;
    }
}
