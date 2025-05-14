using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    [SerializeField] private TMP_Text BulletUI;
    public Camera camera;
    public PickUp pickup;
    private int bulletCount = 30;
    int initialBulletCount;
    void Start()
    {
        int initialBulletCount = bulletCount;
    }

    void Update()
    {

        Debug.Log(bulletCount);
        if (Input.GetKeyDown(KeyCode.Mouse0) && pickup.pickedUpWeapon && bulletCount > 0)
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
        bulletCount = initialBulletCount;
    }

    void updateBulletUI()
    {
        BulletUI.text = "30/" + bulletCount.ToString();
    }

}
