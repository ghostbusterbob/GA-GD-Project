using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemanager : MonoBehaviour
{
    public GameDataManager savingdataplayer;
    public EnemyLocationSaving savingenemylocation;
    public PickUp savingpickup;
    

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            savingdataplayer.SaveGame();
            savingenemylocation.savingdata1();
            savingpickup.SaveInventory();
            switchMainMenu();
        }
    }

    void switchMainMenu()
    {
        SceneManager.LoadScene("UI");
    }
}
