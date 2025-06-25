using UnityEngine;
using UnityEngine.InputSystem.XR;

public class XPscript : MonoBehaviour
{
    public static XPscript instance; //Declare a singleton

    public float CurrentXp;

    void Awake()
    {
        if (instance == null)
        {
            instance = this; //Assign current instance to the singleton
        }
    }
    public void IncreaseXP(float amount)
    {
        CurrentXp += amount;
    }

}