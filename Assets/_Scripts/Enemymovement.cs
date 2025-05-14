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
    }
    private void Update()
    {
           agent.SetDestination(Player.transform.position);
        //object(OnCollisionEnter);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            speed = 0f;
            XPscript.instance.IncreaseXP (1);
        }

        }

}
