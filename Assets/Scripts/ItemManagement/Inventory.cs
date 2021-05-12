using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Dictionary<int, Item> items = new Dictionary<int, Item>();
    public static Dictionary<int, Item> hotbarItems = new Dictionary<int, Item>();
    public static Items itemsFromJSON;
    float length = 5, width = 8;
    float offsetX = 90f, offsetY = 90f;
    public GameObject slot, cameraCanvas, spriteSpawner;
    public static List<GameObject> spawnedSlots = new List<GameObject>();
    public static List<GameObject> spawnedHotbarSlots = new List<GameObject>();
    public static bool inventoryOpened = false;

    void Start()
    {
        spriteSpawner.SetActive(false);
        for (int a = 0; a < length*width; a++)
        {
            items.Add(a, null);
        }

        for (int a = 0; a < 10; a++)
        {
            hotbarItems.Add(a, null);
        }
        LoadItemInfo();
        OpenHotbar();
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

    public static void LoadItemInfo()
    {
        TextAsset jsonData = Resources.Load("JSON/items") as TextAsset;
        itemsFromJSON = JsonUtility.FromJson<Items>(jsonData.text);
    }

    public void OpenHotbar()
    {
        float offsetHorizontal = 0f;
        int slotNumber = 0;

        for (float x = 0; x < 10; x++)
        {
            GameObject spawnedSlot = Instantiate(slot, new Vector3(-400f + 0 + offsetHorizontal, -480f, 0), Quaternion.identity);
            spawnedSlot.transform.SetParent(cameraCanvas.transform, false);
            spawnedSlot.name = "hotbarSlot_" + slotNumber;
            spawnedHotbarSlots.Add(spawnedSlot);
            if (hotbarItems[slotNumber] != null)
            {
                SpawnObjectInSlot(spawnedSlot, hotbarItems[slotNumber].spriteName);
            }
            offsetHorizontal += offsetX;
            slotNumber++;
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
                if (items[slotNumber] != null)
                {
                    SpawnObjectInSlot(spawnedSlot, items[slotNumber].spriteName);
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
        spawnedSlots.Clear();
        SlotManager.grabbedItemSlotID = -1;
        inventoryOpened = false;
    }

    public void CloseHotbar()
    {
        foreach (GameObject slot in spawnedHotbarSlots)
        {
            Destroy(slot);
        }
        spawnedHotbarSlots.Clear();
        SlotManager.grabbedItemSlotID = -1;
    }

    public void ReloadInventory()
    {
        CloseInventory();
        CloseHotbar();
        OpenInventory();
        OpenHotbar();
    }

    public void SpawnObjectInSlot(GameObject parent, string spriteName)
    {
        spriteSpawner.SetActive(true);
        spriteSpawner.GetComponent<Image>().sprite = Resources.Load<Sprite>("Prefabs/InventoryItemSprites/" + spriteName);
        Instantiate(spriteSpawner, parent.transform, false);
        spriteSpawner.SetActive(false);
    }
}
