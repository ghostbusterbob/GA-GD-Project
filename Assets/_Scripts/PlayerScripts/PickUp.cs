using System.Collections;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] GameObject[] inventoryObjects;
    [SerializeField] Camera camera;

    [SerializeField] GameObject PickupDropLocation;
    public LayerMask layerMask;
    private GameObject heldObject;

    private GameObject objectInHand;
    private bool lerping = false;
    private bool addedForce = false;
    private Rigidbody rb;
    private bool pickup = false;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !pickup)
        {
            Pickup();
            pickup = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && pickup)
        {
            Drop();
        }

        if (lerping)
        {
            heldObject.transform.position = Vector3.Lerp(heldObject.transform.position, objectInHand.transform.position, 20f * Time.deltaTime);
            heldObject.transform.rotation = Quaternion.Lerp(heldObject.transform.rotation, objectInHand.transform.rotation, 10f * Time.deltaTime);
            heldObject.transform.localScale = Vector3.Lerp(heldObject.transform.localScale, objectInHand.transform.localScale, 10f * Time.deltaTime);

            float dist = Vector3.Distance(heldObject.transform.position, objectInHand.transform.position);

            if (dist < .01f)
            {
                heldObject.SetActive(false);
                objectInHand.SetActive(true);
            }
        }
        else if (!lerping && rb != null)
        {
            rb.isKinematic = false;

            if(!addedForce)
            rb.AddForce(transform.forward * 200f);
            addedForce = true;

        }

        void Pickup()
        {
            RaycastHit hit;
            LayerMask layerMask = LayerMask.GetMask("Pickup");
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 5f, layerMask))
            {
                string hitName = hit.transform.name;
                foreach (GameObject inventory in inventoryObjects)
                {
                    if (inventory.name == hitName)
                    {
                        objectInHand = inventory;
                    }
                }

                Debug.Log("Picked up: " + hitName);
                heldObject = hit.transform.gameObject;
                lerping = true;
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.isKinematic = true;
            
            }
        }

        void Drop()
        {
            if (heldObject != null)
            {
                lerping = false;
                objectInHand.SetActive(false);
                heldObject.SetActive(true);

                Debug.Log("Dropped: " + heldObject.name);

                heldObject = null;
                pickup = false;
                addedForce= false;


            }
        }
    }
}
