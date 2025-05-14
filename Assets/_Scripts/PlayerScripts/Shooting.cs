using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TMP_Text BulletUI;
    public Camera camera;
    public PickUp pickup;
    private int bulletCount = 30;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(bulletCount);
        if (Input.GetKeyDown(KeyCode.Mouse0)&& pickup.pickedUpWeapon && bulletCount > 0)
        {
            weapon();
            bulletCount -= 1;
            updateBulletUI();
        }
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            reload();
        }
    }
    void weapon()
    {
        LayerMask layerMask = LayerMask.GetMask("Enemy");

        RaycastHit hit;

        if (Physics.Raycast(camera.transform.position, camera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Destroy(hit.transform.gameObject);
        }

    }
    void reload()
    {
        
            bulletCount = 30;
        
    }

    void updateBulletUI()
    {
        BulletUI.text = "30/" + bulletCount.ToString();
    }

}
