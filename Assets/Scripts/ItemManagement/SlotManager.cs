using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    static GameObject cameraObject;
    static GameObject grabbedObject;
    public static int grabbedItemSlotID = -1;
    public static bool grabbedItemSlotInHotbar = false;
    static float grabbedObjectDistance = 0.15f;


    void Start()
    {
        if (grabbedObject == null)
            grabbedObject = new GameObject();

        cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Update()
    {
        if (grabbedItemSlotID != -1)
        {
            MoveGrabbedItem(grabbedObject);
        }
    }

    public void ManageInput(string name)
    {
        if (Input.GetMouseButtonDown(1) && grabbedItemSlotID == -1)
        {
            RemoveItem(Int32.Parse(name.Split('_')[1]), name.Split('_')[0]);
        }
        if (Input.GetMouseButtonDown(0) && grabbedItemSlotID == -1)
        {
            GrabItem(Int32.Parse(name.Split('_')[1]), name.Split('_')[0]);
        }
        else if (Input.GetMouseButtonDown(0) && grabbedItemSlotID != -1)
        {
            ManageMovingObjects(Int32.Parse(name.Split('_')[1]), name.Split('_')[0]);
        }
        if (Input.GetKeyDown(KeyCode.E) && grabbedItemSlotID == -1)
        {
            UseItem(Int32.Parse(name.Split('_')[1]), name.Split('_')[0]);
        }
    }

    public static bool AddItem(int id, int quantityAdded)
    {
        foreach (KeyValuePair<int, Item> key in Inventory.items)
        {
            if (key.Value == null)
            {
                foreach (Item i in Inventory.itemsFromJSON.items)
                {
                    if (id == i.id)
                    {
                        Inventory.items[key.Key] = new Item(id, i.name, i.spriteName, i.prefabName, i.buildablePrefabName, quantityAdded, i.basicPrice, i.buildable, i.consumable);
                        if (Inventory.inventoryOpened)
                            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Inventory>().ReloadInventory();

                        return true;
                    }
                }
            }
        }
        return false;
    }

    public static void UseItem(int slotID, string slotType)
    {
        switch (slotType)
        {
            case "slot":
                if (Inventory.items[slotID] != null && Inventory.items[slotID].consumable && cameraObject.GetComponent<EatingManager>().ConsumeItem(Inventory.items[slotID].id))
                {
                    Inventory.items[slotID] = null;
                    cameraObject.GetComponent<Inventory>().ReloadInventory();
                }
                break;
            case "hotbarSlot":
                if (Inventory.hotbarItems[slotID] != null && Inventory.hotbarItems[slotID].consumable && cameraObject.GetComponent<EatingManager>().ConsumeItem(Inventory.hotbarItems[slotID].id))
                {
                    Inventory.hotbarItems[slotID] = null;
                    cameraObject.GetComponent<Inventory>().ReloadInventory();
                }
                break;
        }
    }

    public static void DestroyItem(int slotID, string slotType)
    {
        switch (slotType)
        {
            case "slot":
                Inventory.items[slotID] = null;
                break;
            case "hotbarSlot":
                Inventory.hotbarItems[slotID] = null;
                break;
        }
    }

    public static void RemoveItem(int slotID, string slotType)
    {
        switch (slotType)
        {
            case "slot":
                if (Inventory.items[slotID] != null)
                {
                    GameObject removedItem;
                    removedItem = Instantiate(Resources.Load<GameObject>("Prefabs/ObjectsDropped/" + Inventory.items[slotID].prefabName), GameObject.FindGameObjectWithTag("Player").transform.position + Camera.main.transform.forward * 0.2f, Quaternion.identity);
                    removedItem.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 160f);
                    Inventory.items[slotID] = null;
                    cameraObject.GetComponent<Inventory>().ReloadInventory();
                }
                break;
            case "hotbarSlot":
                if (Inventory.hotbarItems[slotID] != null)
                {
                    GameObject removedItem;
                    removedItem = Instantiate(Resources.Load<GameObject>("Prefabs/ObjectsDropped/" + Inventory.hotbarItems[slotID].prefabName), GameObject.FindGameObjectWithTag("Player").transform.position + Camera.main.transform.forward * 0.2f, Quaternion.identity);
                    removedItem.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 160f);
                    Inventory.hotbarItems[slotID] = null;
                    cameraObject.GetComponent<Inventory>().ReloadInventory();
                }
                break;
        }


    }

    void GrabItem(int slotID, string slotType)
    {
        switch (slotType)
        {
            case "slot":
                if (Inventory.items[slotID] != null)
                {
                    grabbedObject = transform.Find("spriteSpawner(Clone)").gameObject;
                    transform.SetAsLastSibling();
                    grabbedItemSlotID = slotID;
                    grabbedItemSlotInHotbar = false;
                }
                break;
            case "hotbarSlot":
                if (Inventory.hotbarItems[slotID] != null)
                {
                    grabbedObject = transform.Find("spriteSpawner(Clone)").gameObject;
                    transform.SetAsLastSibling();
                    grabbedItemSlotID = slotID;
                    grabbedItemSlotInHotbar = true;
                }
                break;
        }
    }

    static void MoveGrabbedItem(GameObject grabbedObjectInstance)
    {
        if (grabbedObjectInstance != null)
            grabbedObjectInstance.transform.position = Input.mousePosition;
    }

    void ManageMovingObjects(int selectedItemSlotID, string slotType)
    {
        Item originalItemFromSelectedSlot = slotType == "hotbarSlot" ? Inventory.hotbarItems[selectedItemSlotID] : Inventory.items[selectedItemSlotID];
        if (slotType == "slot")
        {
            Inventory.items[selectedItemSlotID] = grabbedItemSlotInHotbar ? Inventory.hotbarItems[grabbedItemSlotID] : Inventory.items[grabbedItemSlotID];
        }
        else
        {
            Inventory.hotbarItems[selectedItemSlotID] = grabbedItemSlotInHotbar ? Inventory.hotbarItems[grabbedItemSlotID] : Inventory.items[grabbedItemSlotID];
        }

        if (!grabbedItemSlotInHotbar)
        {
            Inventory.items[grabbedItemSlotID] = originalItemFromSelectedSlot;
        }
        else
        {
            Inventory.hotbarItems[grabbedItemSlotID] = originalItemFromSelectedSlot;
        }
        grabbedItemSlotID = -1;
        grabbedItemSlotInHotbar = false;
        cameraObject.GetComponent<Inventory>().ReloadInventory();
    }
}
