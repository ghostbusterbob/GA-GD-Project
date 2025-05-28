using System.IO;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public static test instance;

    [SerializeField] private Text LevelText;
    [SerializeField] private Text ExperienceText;
    [SerializeField] private int Level;

    private float CurrentXp;
    [SerializeField] private float TartgetXp = 100; // Default starting XP needed

    public Slider XpProgressBar;

    private string saveFilePath;

    [System.Serializable]
public class XpSaveData
{
    public float currentXp;
    public int level;
    public float targetXp;
}

    private void Awake()
    {
        instance = this;
        saveFilePath = Path.Combine(Application.persistentDataPath, "xp_save.json");
        LoadXpData();
    }

    private void OnApplicationQuit()
    {
        SaveXpData();
    }

    void Update()
    {
        ExperienceText.text = CurrentXp + " / " + TartgetXp;
        ExperienceController();
    }

    public void ExperienceController()
    {
        LevelText.text = Level.ToString();
        XpProgressBar.value = (CurrentXp / TartgetXp);

        if (CurrentXp >= TartgetXp)
        {
            CurrentXp -= TartgetXp;
            Level++;
            TartgetXp += 50;
        }
    }

    public void AddXpOnEnemyDeath()
    {
        CurrentXp += 15;
        ExperienceController();
    }

    public void SaveXpData()
    {
        XpSaveData data = new XpSaveData
        {
            currentXp = CurrentXp,
            level = Level,
            targetXp = TartgetXp
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("XP Data Saved");
    }

    public void LoadXpData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            XpSaveData data = JsonUtility.FromJson<XpSaveData>(json);

            CurrentXp = data.currentXp;
            Level = data.level;
            TartgetXp = data.targetXp;

            Debug.Log("XP Data Loaded");
        }
        else
        {
            Debug.Log("No save file found, starting fresh.");
        }
    }
}