using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;


[System.Serializable]
public class XpSaveData
{
    public float currentXp;
    public int level;
    public float targetXp;
}


public class XPsystem : MonoBehaviour
{
    public static XPsystem instance;

    [SerializeField] private Text LevelText;
    [SerializeField] private Text ExperienceText;
    [SerializeField] private int Level;
    private float CurrentXp;
    [SerializeField] private float TartgetXp = 100;

    public Slider XpProgressBar;

    private string saveFilePath;

    private void Awake()
    {
        instance = this;
        saveFilePath = Path.Combine(Application.persistentDataPath, "xp_save.txt");
        LoadXpData();
    }
    private void Start()
    {
        LoadXpData();
    }

    private void OnApplicationQuit()
    {
        SaveXpData();
    }

    void Update()
    {
        ExperienceText.text = "Expirience: " + CurrentXp + " / " + TartgetXp;
        ExperienceController();
    }

    public void ExperienceController()
    {
        //LevelText.text = Level.ToString();
        LevelText.text = "Level: " + Level.ToString();
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
        }
        else
        {
            Debug.Log("No save file found, starting fresh.");
        }
    }
    public void ResetXpData()
    {
        CurrentXp = 0;
        Level = 1;
        TartgetXp = 100;
        SaveXpData();
        Debug.Log("XP Data Reset");
    }
    [System.Serializable]
    public class XpSaveData
    {
        public float currentXp;
        public int level;
        public float targetXp;
    }
}