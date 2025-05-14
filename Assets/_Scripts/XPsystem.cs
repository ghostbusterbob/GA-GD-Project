using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class XPsystem : MonoBehaviour
{
    public static XPsystem instance;
    [SerializeField] private Text LevelText;
    [SerializeField] private Text ExperienceText;
    [SerializeField] private int Level;
    public float CurrentXp;
    [SerializeField] private float TartgetXp;
    public Slider XpProgressBar;
    void Update()
    {
        CurrentXp = 0;
        //make on kill with enemy to add xp yes
        ExperienceText.text = CurrentXp + " / " + TartgetXp;
        ExperienceController();
    }

    public void ExperienceController()
    {
        LevelText.text = "" + Level.ToString();
        XpProgressBar.value = (CurrentXp / TartgetXp);

        if (CurrentXp >= TartgetXp)
        {
            CurrentXp = CurrentXp - TartgetXp;
            Level++;
            TartgetXp += 50;
        }
    }
    public void AddXpOnEnemyDeath()
    {
        CurrentXp += 15;
        ExperienceController();
    }
}