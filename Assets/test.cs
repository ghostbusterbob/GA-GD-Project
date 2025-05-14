using UnityEngine;
using UnityEngine.InputSystem.XR;

public class test : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        XPsystem.instance.AddXpOnEnemyDeath();
    }
}