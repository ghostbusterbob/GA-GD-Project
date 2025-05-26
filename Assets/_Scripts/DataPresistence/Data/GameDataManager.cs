using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using System.Text;
using Unity.VisualScripting;
public class PlayerData
{
    public float[] position;
    public int health;
    public int score;
    public int XPsystem;
    public Vector3 playerposition;
}
public class GameDataManager : MonoBehaviour
{
    public Transform playerTransform;
    public int health;
    public int xpsystem;
    public Vector3 playerposition;

    public void SaveGame()
    {
        PlayerData playerData = new PlayerData
        {
            //position = new Vector3[] { playerTransform.position.x, playerTransform.position.y, playerTransform.position.z },
            playerposition = new Vector3(playerTransform.position.x , playerTransform.position.y , playerTransform.position.z),   
            health = this.health,
            // score = ScoreManager.scoreCount,
            //level = LevelManager.levelCount,
            //XPsystem = XPsystem.instance.xpsystem
        };

        string json = JsonUtility.ToJson(playerData);
        string path = Application.persistentDataPath + "data/playerData.json";
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
