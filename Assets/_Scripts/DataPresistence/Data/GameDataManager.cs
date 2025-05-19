using System.IO;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public Transform playerTransform;
    public int health;
    public int score;

    public void SaveGame()
    {
        PlayerData playerData = new PlayerData();
        playerData.position = new float[] { playerTransform.position.x, playerTransform.position.y, playerTransform.position.z };
        playerData.health = GameDataManager.health;
        playerData.score = ScoreManager.scoreCount;

        string json = JsonUtility.ToJson(playerData);
        string path = Application.persistentDataPath + ".playerData.json";
        System.IO.File.WriteAllText(path, json);
    }

    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/playerData.json";
        if (File.Exists(path)) 
        {
           string json = System.IO.File.ReadAllText(path);
            playerData loadedData = JsonUtility.FromJson<PlayerData>(json);

            //update player's transform position
            playerTransform.position = new Vector3(loadedData.position[0], loadedData.position[1], loadedData.position[2]);
            
        }

    }
}
