using UnityEngine;

public class Shooting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Camera camera;
    public PickUp pickup;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            weapon();
        }
    }
    void weapon()
    {
        LayerMask layerMask = LayerMask.GetMask("Enemy");

        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(camera.transform.position, camera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask)&& pickup.pickedUpWeapon) 
            {
                Destroy(hit.transform.gameObject);  
            }
        }
    }
}
