using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    // Reference to the button in the Inspector
    public Button playButton;

    // Name of the scene you want to load (set this in the Inspector)
    public string start_butten_scene_To_Load;

    void Start()
    {
        // Add a listener to the button so it calls the LoadScene method when clicked
        playButton.onClick.AddListener(LoadScene);
    }
    public string loadlevel(int sceneindex)
    {
        return start_butten_scene_To_Load;
    
    }
    public void loadtestmap(int sceneindex)
    {
        start_butten_scene_To_Load = "testmap";
    }

    void LoadScene()
    {
        Debug.Log("starting to load" + start_butten_scene_To_Load);
        SceneManager.LoadScene(start_butten_scene_To_Load);
    }
}