using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.XR;


public class Enemymovement : MonoBehaviour
{
    public GameObject Player;
    float speed = 3f;
    private Animator animator;
    public NavMeshAgent agent;



    private void Start()
    {
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
            if(agent != null)
           agent.SetDestination(Player.transform.position);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject == Player)
        {
            
            Debug.Log($"doe het pls");
        }
    }

}
