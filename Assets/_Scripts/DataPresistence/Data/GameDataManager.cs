using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerData
{
    public float[] position;
    public int health;
    public int score;
    public int xpSYSTEM;
}
public class GameDataManager : MonoBehaviour
{
    public Transform playerTransform;
    public int health;
    public int currentxplevel;

    public void SaveGame()
    {
        PlayerData playerData = new PlayerData
        {
            position = new float[] { playerTransform.position.x, playerTransform.position.y, playerTransform.position.z },
            health = this.health,
            // score = ScoreManager.scoreCount,
            //level = LevelManager.levelCount,
            //xpSYSTEM = XPsystem.instance.currentxplevel
        };

        string json = JsonUtility.ToJson(playerData);
        string path = Application.persistentDataPath + "/playerData.json";
        System.IO.File.WriteAllText(path, json);
    }

    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/playerData.json";
        if (File.Exists(path)) 
        {
           string json = System.IO.File.ReadAllText(path);
            PlayerData loadedData = JsonUtility.FromJson<PlayerData>(json);

            //update player's transform position
            playerTransform.position = new Vector3(loadedData.position[0], loadedData.position[1], loadedData.position[2]);
            
        }

    }
}
