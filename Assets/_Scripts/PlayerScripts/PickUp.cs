    using System.Collections.Generic;
    using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
    using static UnityEditor.Progress;

    public class PickUp : MonoBehaviour
    {
        [SerializeField] GameObject[] inventoryObjects;
        [SerializeField] Camera camera;
        [SerializeField] GameObject PickupDropLocation;
        [SerializeField] LineRenderer render;

        public LayerMask layerMask;

        public List<GameObject> heldObjects = new List<GameObject>();
        public List<GameObject> inventoryInHands = new List<GameObject>();
        public Transform[] inventoryLocations;
        public static PickUp pickupsavingsystem;

        private int currentItemIndex = -1;
        private bool lerping = false;
        private bool handLerping = false;
        private bool addedForce = false;
        private bool pickup = false;
        private Rigidbody rb;
        private string saveFilePath1;
        public bool pickedUpWeapon = false;

        private void Awake()
        {
            pickupsavingsystem = this;
            saveFilePath1 = Path.Combine(Application.persistentDataPath, "pickup.txt");
        }
        public void savingsystemitems()
        {
            savingitem();
        }
    public void Start()
    {

        LoadItems();
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
            else if (Input.GetKeyDown(KeyCode.R))
            {
                SwitchItem(1);
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
            }
        }
    }


    void Drop()
    {
        if (currentItemIndex >= 0 && currentItemIndex < heldObjects.Count)
        {
            GameObject heldObject = heldObjects[currentItemIndex];
            GameObject inventoryItem = inventoryInHands[currentItemIndex];
            heldObject.transform.position = inventoryItem.transform.position;
            heldObject.SetActive(true);
            inventoryItem.SetActive(false);
            rb = heldObject.GetComponent<Rigidbody>();
            if (rb == null) rb = heldObject.AddComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(transform.forward * 200f);
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
            }
            addedForce = false;
            savingitem();
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
        }
        private void savingitem()
        {
            if (heldObjects.Count > 0)
            {
                using (StreamWriter writer = new StreamWriter(saveFilePath1))
                {
                    foreach (GameObject item in heldObjects)
                    {
                        Vector3 pos = item.transform.position;
                        string line = $"{item.name},{pos.x},{pos.y},{pos.z}";
                        writer.WriteLine(line);
                    }
                }
            }
        }

    private void LoadItems()
    {
        if (!File.Exists(saveFilePath1))
        {
            Debug.LogWarning("No save file found.");
            return;
        }
        string[] lines = File.ReadAllLines(saveFilePath1);
        HashSet<string> itemNamesInInventory = new HashSet<string>();

        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            if (parts.Length == 4)
            {
                string itemName = parts[0];
                float x = float.Parse(parts[1]);
                float y = float.Parse(parts[2]);
                float z = float.Parse(parts[3]);
                itemNamesInInventory.Add(itemName);
                GameObject prefab = System.Array.Find(inventoryObjects, obj => obj.name == itemName);
                if (prefab != null)
                {
                    GameObject instance = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity);
                    instance.name = prefab.name;

                    heldObjects.Add(instance);
                    inventoryInHands.Add(prefab);
                    prefab.SetActive(false);
                }
            }
        }
        GameObject[] allSceneObjects = GameObject.FindGameObjectsWithTag("Pickup");
        foreach (GameObject obj in allSceneObjects)
        {
            if (itemNamesInInventory.Contains(obj.name))
            {
                obj.AddComponent<Rigidbody>();
                obj.SetActive(false);
                Debug.Log($"Set inactive (in inventory): {obj.name}");
            }
        }
        if (inventoryInHands.Count > 0)
        {
            currentItemIndex = 0;
            inventoryInHands[0].SetActive(true);
        }
    }
}
