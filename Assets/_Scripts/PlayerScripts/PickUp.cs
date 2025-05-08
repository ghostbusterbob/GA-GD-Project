using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    string tag;
    [SerializeField] GameObject[] inventoryObjects;
    [SerializeField] Camera camera;

    [SerializeField] GameObject PickupDropLocation;

    public LayerMask layerMask;

    private GameObject heldObject;
    private GameObject objectInHand;


    private bool pickup = false;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && pickup == false)
        {
            Pickup();
            pickup = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && pickup == true)
        {
            Drop();
        }

        
        
    }

    void Pickup()
    {

        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 10f))
        {
            string hitName = hit.transform.name;

        foreach (GameObject inventory in inventoryObjects)
        {
            if (inventory.name == hitName)
            {
                inventory.SetActive(true);
            }
        }

        Debug.Log("Picked up: " + hitName);

        heldObject = hit.transform.gameObject;
        heldObject.SetActive(false); // Hide it in world
        heldObject.transform.position = camera.transform.position;

            



        }
    }


    void Drop()
    {
        if (heldObject != null)
        {
            objectInHand.transform.gameObject.SetActive(false);
            heldObject.transform.position = camera.transform.position + camera.transform.forward * 2f;
            heldObject.SetActive(true);
            Debug.Log("Dropped: " + heldObject.name);
            heldObject = null;
            pickup = false;
        }
    }

}
