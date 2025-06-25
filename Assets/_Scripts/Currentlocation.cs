using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Currentlocation : MonoBehaviour
{
    [SerializeField] private Text currentLocation;

    private void Awake()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        currentLocation.text = "Current Location: " + sceneName;
    }
}
