using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    public static HealthSystem instanceHealth;
    private float maxHealth = 1000f;
    public float currentHealth;
    private float damage = 1f;
    private float healing = 5f;
    public Slider healthSlider;
    public XPsystem xpSystem;
    private GameObject player;
    public TMPro.TextMeshProUGUI healthText;
    [SerializeField] private EnemyLocationSaving enemyLocationSaving;


    private string saveFilePath;

    private void Awake()
    {
        instanceHealth = this;
        saveFilePath = Path.Combine(Application.persistentDataPath, "Healthsave.txt");
        LoadHealthData();
    }
    private void Start()
    {
        LoadHealthData();
    }
    private void Update()
    {
        healthText.text = " Health " + currentHealth;
        healthSlider.value = currentHealth / maxHealth;
        if (currentHealth <= 1)
        {
            deathhealth();
        }
    }
    private void OnApplicationQuit()
    {
        SaveHealthData();
    }

    private void ApplyHealing()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healing;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            Debug.Log("Healed. Current health: " + currentHealth);
        }
    }

    private void DamageHealth()
    {
        if (currentHealth >= 0)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Max(currentHealth, 0);
            Debug.Log("Damaged. Current health: " + currentHealth);
        }
    }

    public void SaveHealthData()
    {
        Healthsavedata data = new Healthsavedata
        {
            currentHealth = currentHealth
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
    }

    public void LoadHealthData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            Healthsavedata data = JsonUtility.FromJson<Healthsavedata>(json);

            currentHealth = data.currentHealth;
        }
        else
        {
            Debug.Log("No save file found, starting fresh.");
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy");
            DamageHealth();
        }
    }
    public class Healthsavedata
    {
        public float currentHealth;
    }
    public void deathhealth()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead");
            backToMainMenu();
            SaveHealthData();
            XPsystem.instance.ResetXpData();
            player.SetActive(false);

        }
    }

    void backToMainMenu()
    {
                SceneManager.LoadScene("UI");

        currentHealth = currentHealth + 100f;
        enemyLocationSaving.resetwaves();


    }
}
