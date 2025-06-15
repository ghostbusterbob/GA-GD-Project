using System.Collections.Generic;
using UnityEngine;
using System.IO; // For File IO
using System;

[System.Serializable]
public class SaveData
{
    public List<string> pickedUpObjectNames = new List<string>();
}


public class PickUp : MonoBehaviour
{
    [SerializeField] GameObject[] inventoryObjects;
    [SerializeField] Camera camera;
    [SerializeField] GameObject PickupDropLocation;
    [SerializeField] LineRenderer render;

    public LayerMask layerMask;

    public List<GameObject> heldObjects = new List<GameObject>();
    public List<GameObject> inventoryInHands = new List<GameObject>();

    private int currentItemIndex = -1;
    private bool lerping = false;
    private bool handLerping = false;
    private bool addedForce = false;
    private bool pickup = false;
    private Rigidbody rb;

    public bool pickedUpWeapon = false;

    void Start()
    {
        LoadInventory();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && heldObjects.Count < 3)
        {
            Pickup();
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            Drop();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchItem(-1);
        }
        

        if (lerping && currentItemIndex >= 0)
        {
            GameObject heldObject = heldObjects[currentItemIndex];
            GameObject objectInHand = inventoryInHands[currentItemIndex];

            heldObject.transform.position = Vector3.Lerp(heldObject.transform.position, objectInHand.transform.position, 10f * Time.deltaTime);
            heldObject.transform.rotation = Quaternion.Lerp(heldObject.transform.rotation, objectInHand.transform.rotation, 10f * Time.deltaTime);

            float dist = Vector3.Distance(heldObject.transform.position, objectInHand.transform.position);
            if (dist < 0.1f)
            {
                heldObject.SetActive(false);
                objectInHand.SetActive(true);
                lerping = false;
                handLerping = false;
                pickup = true;
            }
        }
        else if (!lerping && rb != null)
        {
            rb.isKinematic = false;
            if (!addedForce)
            {
                rb.AddForce(transform.forward * 200f);
                addedForce = true;
            }
        }
    }

    void Pickup()
    {
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("Pickup");
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 3f, layerMask))
        {
            GameObject target = hit.transform.gameObject;

            GameObject matchInventory = null;
            foreach (GameObject inventory in inventoryObjects)
            {
                if (inventory.name == target.name)
                {
                    matchInventory = inventory;
                    break;
                }
            }

            if (matchInventory != null)
            {
                // Disable previously active inventory item, if any
                if (currentItemIndex >= 0 && currentItemIndex < inventoryInHands.Count)
                {
                    inventoryInHands[currentItemIndex].SetActive(false);
                }

                inventoryInHands.Add(matchInventory);
                heldObjects.Add(target);

                matchInventory.SetActive(false);

                rb = target.GetComponent<Rigidbody>();
                rb.isKinematic = true;

                currentItemIndex = heldObjects.Count - 1;

                handLerping = true;
                lerping = true;
                pickup = true;

                if (matchInventory.tag == "weapon")
                    pickedUpWeapon = true;

                Debug.Log("Picked up: " + target.name);
            }
        }
    }


    void Drop()
    {
        if (currentItemIndex >= 0 && currentItemIndex < heldObjects.Count)
        {
            GameObject heldObject = heldObjects[currentItemIndex];
            GameObject inventoryItem = inventoryInHands[currentItemIndex];


            heldObject.gameObject.transform.position = inventoryItem.transform.position;
            heldObject.SetActive(true);
            inventoryItem.SetActive(false);

            rb = heldObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(transform.forward * 200f);

            Debug.Log("Dropped: " + heldObject.name);

            heldObjects.RemoveAt(currentItemIndex);
            inventoryInHands.RemoveAt(currentItemIndex);

            if (heldObjects.Count > 0)
            {
                currentItemIndex = 0;
                inventoryInHands[currentItemIndex].SetActive(true);
            }
            else
            {
                currentItemIndex = -1;
                pickup = false;
            }

            addedForce = false;
        }
    }

    void SwitchItem(int direction)
    {
        if (heldObjects.Count == 0) return;

        if (currentItemIndex >= 0)
            inventoryInHands[currentItemIndex].SetActive(false);

        currentItemIndex += direction;

        if (currentItemIndex >= heldObjects.Count) currentItemIndex = 0;
        else if (currentItemIndex < 0) currentItemIndex = heldObjects.Count - 1;

        inventoryInHands[currentItemIndex].SetActive(true);

        Debug.Log("Switched to: " + inventoryInHands[currentItemIndex].name);
    }
    public void SaveInventory()
    {
        SaveData data = new SaveData();

        foreach (GameObject obj in heldObjects)
        {
            data.pickedUpObjectNames.Add(obj.name);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/inventorySave.json", json);

        Debug.Log("Inventory saved.");
    }

    public void LoadInventory()
    {
        string path = Application.persistentDataPath + "/inventorySave.json";
        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        foreach (string name in data.pickedUpObjectNames)
        {
            // Find matching object in the scene
            GameObject sceneObject = GameObject.Find(name);
            if (sceneObject != null)
            {
                GameObject matchInventory = null;
                foreach (GameObject inventory in inventoryObjects)
                {
                    if (inventory.name == sceneObject.name)
                    {
                        matchInventory = inventory;
                        break;
                    }
                }

                if (matchInventory != null)
                {
                    matchInventory.SetActive(false);
                    sceneObject.SetActive(false);

                    heldObjects.Add(sceneObject);
                    inventoryInHands.Add(matchInventory);
                }
            }
        }

        // Activate first item if available
        if (inventoryInHands.Count > 0)
        {
            currentItemIndex = 0;
            inventoryInHands[0].SetActive(true);
        }

        Debug.Log("Inventory loaded.");
    }
}