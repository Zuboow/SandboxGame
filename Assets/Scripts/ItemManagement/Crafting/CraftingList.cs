using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingList : MonoBehaviour
{
    public static Dictionary<int, Item> craftingOptions = new Dictionary<int, Item>();
    public static List<GameObject> spawnedSlots = new List<GameObject>();
    float length = 5, width = 8;
    float offsetX = 90f, offsetY = 90f;
    public GameObject slot, cameraCanvas, spriteSpawner;
    bool craftingListOpened = false;

    private void OnEnable()
    {
        for (int a = 0; a < length * width; a++)
        {
            craftingOptions.Add(a, null);
        }
    }

    private void FixedUpdate()
    {
        //testing
        if (Inventory.itemsFromJSON != null && craftingOptions[0] == null && craftingOptions[4] == null)
        {
            craftingOptions[0] = Inventory.itemsFromJSON.items[0];
            craftingOptions[1] = Inventory.itemsFromJSON.items[6];
        }
        //

        if (Inventory.inventoryOpened)
        {
            if (!craftingListOpened)
            {
                OpenCraftingList();
            }
        } else
        {
            if (craftingListOpened)
            {
                CloseCraftingList();
            }
        }
    }

    public void OpenCraftingList()
    {
        float offsetHorizontal = 0f;
        float offsetVertical = 0f;
        int slotNumber = 0;

        for (float x = 0; x < width; x++)
        {
            for (float y = 0; y < length; y++)
            {
                GameObject spawnedSlot = Instantiate(slot, new Vector3(420f + 0 + offsetHorizontal, 300f + 0 + offsetVertical, 0), Quaternion.identity);
                spawnedSlot.transform.SetParent(cameraCanvas.transform, false);
                spawnedSlot.name = "craftingSlot_" + slotNumber;
                spawnedSlots.Add(spawnedSlot);
                if (craftingOptions[slotNumber] != null)
                {
                    SpawnObjectInSlot(spawnedSlot, craftingOptions[slotNumber].spriteName);
                }
                offsetHorizontal += offsetX;
                slotNumber++;
            }
            offsetHorizontal = 0f;
            offsetVertical -= offsetY;
        }
        craftingListOpened = true;
    }

    public void CloseCraftingList()
    {
        foreach (GameObject slot in spawnedSlots)
        {
            Destroy(slot);
        }
        spawnedSlots.Clear();
        craftingListOpened = false;
    }

    public void ReloadCraftingList()
    {
        CloseCraftingList();
        OpenCraftingList();
    }

    public void SpawnObjectInSlot(GameObject parent, string spriteName)
    {
        spriteSpawner.SetActive(true);
        spriteSpawner.GetComponent<Image>().sprite = Resources.Load<Sprite>("Prefabs/InventoryItemSprites/" + spriteName);
        Instantiate(spriteSpawner, parent.transform, false);
        spriteSpawner.SetActive(false);
    }
}
