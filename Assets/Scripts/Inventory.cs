using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Dictionary<int, int> items = new Dictionary<int, int>();
    float length = 6, width = 8;
    float offsetX = 90f, offsetY = 90f;
    public GameObject slot, cameraCanvas, spriteSpawner;
    List<GameObject> spawnedSlots = new List<GameObject>();
    public static bool inventoryOpened = false;

    void Start()
    {
        spriteSpawner.SetActive(false);
        for (int a = 0; a < length*width; a++)
        {
            items.Add(a, -1);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            switch (inventoryOpened)
            {
                case true:
                    CloseInventory();
                    Cursor.lockState = CursorLockMode.Locked;
                    break;
                case false:
                    OpenInventory();
                    Cursor.lockState = CursorLockMode.None;
                    break;
            }
        }
    }

    public void OpenInventory()
    {
        float offsetHorizontal = 0f;
        float offsetVertical = 0f;
        int slotNumber = 0;

        for (float x = 0; x < width; x++)
        {
            for (float y = 0; y < length; y++)
            {
                GameObject spawnedSlot = Instantiate(slot, new Vector3(-800f + 0 + offsetHorizontal, 300f + 0 + offsetVertical, 0), Quaternion.identity);
                spawnedSlot.transform.SetParent(cameraCanvas.transform, false);
                spawnedSlot.name = "slot_" + slotNumber;
                spawnedSlots.Add(spawnedSlot);
                if (items[slotNumber] != -1)
                {
                    SpawnObjectInSlot(spawnedSlot);
                }
                offsetHorizontal += offsetX;
                slotNumber++;
            }
            offsetHorizontal = 0f;
            offsetVertical -= offsetY;
        }

        inventoryOpened = true;
    }

    public void CloseInventory()
    {
        foreach (GameObject slot in spawnedSlots)
        {
            Destroy(slot);
        }
        spawnedSlots = new List<GameObject>();

        inventoryOpened = false;
    }

    public void ReloadInventory()
    {
        CloseInventory();
        OpenInventory();
    }

    public void SpawnObjectInSlot(GameObject parent)
    {
        spriteSpawner.SetActive(true);
        spriteSpawner.GetComponent<Image>().sprite = Resources.Load<Sprite>("Prefabs/ObjectsInInventory/" + "block01_"); //testing purposes
        Instantiate(spriteSpawner, parent.transform, false);
        spriteSpawner.SetActive(false);
    }
}
