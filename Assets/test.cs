using UnityEngine;
using UnityEngine.InputSystem.XR;

public class test : MonoBehaviour
{
    public static test instance; //Declare a singleton

    private float CurrentXp;

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
