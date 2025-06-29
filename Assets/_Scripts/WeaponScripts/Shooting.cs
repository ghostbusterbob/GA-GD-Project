using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    [SerializeField] private TMP_Text BulletUI;
    public Camera camera;
    public PickUp pickup;
    public int bulletCount = 30;
    public XPsystem xp;
    public Spawner Spawner;
    public EnemyLocationSaving locationSaving;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private CharacterController controller;
    


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && pickup.pickedUpWeapon && bulletCount >= 0)
        {
            weapon();
            updateBulletUI();

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            reload();
        }

        
    }


    public void weapon()
    {
        LayerMask layerMask = LayerMask.GetMask("Enemy");
        RaycastHit hit;

        if (bulletCount >= 0)
        {
            updateBulletUI();
            bulletCount -= 1;

            if (Physics.Raycast(camera.transform.position, camera.transform.TransformDirection(Vector3.forward), out hit, 50f, layerMask))
            {
                Debug.Log("Hit");
                xp.AddXpOnEnemyDeath();
                locationSaving.killEnemy();
                Destroy(hit.transform.gameObject);
            }
        }
    }

     

    void reload()
    {
        bulletCount = 30;
        updateBulletUI();
    }

    void updateBulletUI()
    {
        BulletUI.text = "30/" + bulletCount.ToString();
    }
}
