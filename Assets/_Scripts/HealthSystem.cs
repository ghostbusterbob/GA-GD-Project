using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public static HealthSystem instanceHealth;
    private float maxHealth = 100f;
    private float currentHealth;
    private float damage = 10f;
    private float healing = 5f;
    public TMPro.TextMeshProUGUI healthText;

    private string saveFilePath;

    private void Awake()
    {
        instanceHealth = this;
        saveFilePath = Path.Combine(Application.persistentDataPath, "Healthsave.txt");
        LoadHealthData();
    }
    private void Update()
    {
        healthText.text = " Health " + currentHealth;
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
        if (currentHealth > 0)
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
        Debug.Log("XP Data Saved");
    }

    public void LoadHealthData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            Healthsavedata data = JsonUtility.FromJson<Healthsavedata>(json);

            currentHealth = data.currentHealth;

            Debug.Log("XP Data Loaded");
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
    
}
