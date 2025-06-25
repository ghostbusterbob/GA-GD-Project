using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyCombatscript : MonoBehaviour
{
    public GameObject player;
    public UnityEvent Healthevent;

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("A");
        Healthevent.Invoke();
    }
}
