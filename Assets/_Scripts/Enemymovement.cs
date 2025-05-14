using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Controls;


public class Enemymovement : MonoBehaviour
{
    public GameObject Player;
    float speed = 3f;

    private void Update()
    {
           transform.LookAt(Player.transform.position);
           transform.Translate(0, 0, speed * Time.deltaTime);

    }
    private void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.name == "Player")
        {
            speed = 0f;
        }

        }
}
