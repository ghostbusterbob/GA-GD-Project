using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Controls;


public class Enemymovement : MonoBehaviour
{
    public GameObject Player;
    float speed = 3f;
    private Animator animator;
    public NavMeshAgent agent;  


    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
           agent.SetDestination(Player.transform.position);

    }
    private void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.name == "Player")
        {
            speed = 0f;
            
        }

        }
}
