using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    static GameObject cameraObject;
    static GameObject grabbedObject;
    public static Item grabbedItem;
    public static int grabbedItemSlotID = -1;
    public static int grabbedItemQuantity = 0;
    public static bool grabbedItemSlotInHotbar = false;
    static float grabbedObjectDistance = 0.15f;
    public static int quantityLimitPerSlot = 5;

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
        if ((Input.GetKeyDown(KeyCode.O) || Input.GetMouseButtonDown(1)) && grabbedItemSlotID == -1)
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

    public static int AddItem(int id, int quantityAdded)
    {
        int quantityLeft = quantityAdded;
        quantityLeft = TryToFillStacksWithItem(Inventory.items, id, quantityLeft);

        if (quantityLeft > 0)
            quantityLeft = TryToFillStacksWithItem(Inventory.hotbarItems, id, quantityLeft);

        if (quantityLeft > 0)
            quantityLeft = TryToAddItemInSpecificQuantity(Inventory.items, id, quantityLeft);

        if (quantityLeft > 0)
            quantityLeft = TryToAddItemInSpecificQuantity(Inventory.hotbarItems, id, quantityLeft);


        if (quantityLeft != quantityAdded && Inventory.inventoryOpened)
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Inventory>().ReloadInventory();

        return quantityLeft;
    }

    static int TryToFillStacksWithItem(Dictionary<int, Item> itemList, int id, int quantityLeft)
    {
        foreach (KeyValuePair<int, Item> key in itemList)
        {
            if (key.Value != null)
            {
                if (key.Value.id == id)
                {
                    if (key.Value.quantity + quantityLeft >= quantityLimitPerSlot)
                    {
                        quantityLeft -= quantityLimitPerSlot - key.Value.quantity;
                        key.Value.quantity = quantityLimitPerSlot;
                    }
                    else
                    {
                        key.Value.quantity += quantityLeft;
                        return 0;
                    }
                }
            }
        }
        return quantityLeft;
    }

    static int TryToAddItemInSpecificQuantity(Dictionary<int, Item> itemList, int id, int quantityLeft)
    {
        foreach (KeyValuePair<int, Item> key in itemList.ToList())
        {
            if (key.Value == null)
            {
                foreach (Item i in Inventory.itemsFromJSON.items)
                {
                    if (id == i.id)
                    {
                        itemList[key.Key] = new Item(id, i.name, i.spriteName, i.prefabName, i.buildablePrefabName, quantityLeft <= quantityLimitPerSlot ? quantityLeft : quantityLimitPerSlot, i.basicPrice, i.buildable, i.consumable);
                        quantityLeft = quantityLeft <= quantityLimitPerSlot ? 0 : quantityLeft - quantityLimitPerSlot;
                        if (quantityLeft <= 0)
                            return 0;
                    }
                }
            }
        }
        return quantityLeft;
    }

    public static void UseItem(int slotID, string slotType)
    {
        switch (slotType)
        {
            case "slot":
                if (Inventory.items[slotID] != null && Inventory.items[slotID].consumable && cameraObject.GetComponent<EatingManager>().ConsumeItem(Inventory.items[slotID].id))
                {
                    if (Inventory.items[slotID].quantity - 1 == 0)
                        Inventory.items[slotID] = null;
                    else
                        Inventory.items[slotID].quantity -= 1;
                    cameraObject.GetComponent<Inventory>().ReloadInventory();
                }
                break;
            case "hotbarSlot":
                if (Inventory.hotbarItems[slotID] != null && Inventory.hotbarItems[slotID].consumable && cameraObject.GetComponent<EatingManager>().ConsumeItem(Inventory.hotbarItems[slotID].id))
                {
                    if (Inventory.hotbarItems[slotID].quantity - 1 == 0)
                        Inventory.hotbarItems[slotID] = null;
                    else
                        Inventory.hotbarItems[slotID].quantity -= 1;
                    cameraObject.GetComponent<Inventory>().ReloadInventory();
                }
                break;
        }
    }

    public static void DestroyItem(int slotID, string slotType, int amountDestroyed)
    {
        switch (slotType)
        {
            case "slot":
                if (Inventory.items[slotID].quantity - amountDestroyed <= 0)
                    Inventory.items[slotID] = null;
                else
                    Inventory.items[slotID].quantity -= amountDestroyed;
                break;
            case "hotbarSlot":
                if (Inventory.hotbarItems[slotID].quantity - amountDestroyed <= 0)
                    Inventory.hotbarItems[slotID] = null;
                else
                    Inventory.hotbarItems[slotID].quantity -= amountDestroyed;
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
                    if (Inventory.items[slotID].quantity - 1 == 0)
                        Inventory.items[slotID] = null;
                    else
                        Inventory.items[slotID].quantity -= 1;
                    cameraObject.GetComponent<Inventory>().ReloadInventory();
                }
                break;
            case "hotbarSlot":
                if (Inventory.hotbarItems[slotID] != null)
                {
                    GameObject removedItem;
                    removedItem = Instantiate(Resources.Load<GameObject>("Prefabs/ObjectsDropped/" + Inventory.hotbarItems[slotID].prefabName), GameObject.FindGameObjectWithTag("Player").transform.position + Camera.main.transform.forward * 0.2f, Quaternion.identity);
                    removedItem.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 160f);
                    if (Inventory.hotbarItems[slotID].quantity - 1 == 0)
                        Inventory.hotbarItems[slotID] = null;
                    else
                        Inventory.hotbarItems[slotID].quantity -= 1;
                    cameraObject.GetComponent<Inventory>().ReloadInventory();
                }
                break;
        }
    }

    public static void ThrowItemOut(int itemID)
    {
        foreach (Item i in Inventory.itemsFromJSON.items)
        {
            if (itemID == i.id)
            {
                GameObject removedItem = Instantiate(Resources.Load<GameObject>("Prefabs/ObjectsDropped/" + i.prefabName), GameObject.FindGameObjectWithTag("Player").transform.position + Camera.main.transform.forward * 0.2f, Quaternion.identity);
                removedItem.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 160f);
            }
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
                    transform.Find("Quantity").GetComponent<TextMeshProUGUI>().text = "";
                    transform.SetAsLastSibling();
                    grabbedItemSlotID = slotID;
                    grabbedItem = Inventory.items[slotID];
                    grabbedItemQuantity = Inventory.items[slotID].quantity;
                    grabbedItemSlotInHotbar = false;
                }
                break;
            case "hotbarSlot":
                if (Inventory.hotbarItems[slotID] != null)
                {
                    grabbedObject = transform.Find("spriteSpawner(Clone)").gameObject;
                    transform.Find("Quantity").GetComponent<TextMeshProUGUI>().text = "";
                    transform.SetAsLastSibling();
                    grabbedItemSlotID = slotID;
                    grabbedItem = Inventory.hotbarItems[slotID];
                    grabbedItemQuantity = Inventory.hotbarItems[slotID].quantity;
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

        bool replaceItems = false;
        if (originalItemFromSelectedSlot != null && (!grabbedItemSlotInHotbar ? Inventory.items[grabbedItemSlotID].id : Inventory.hotbarItems[grabbedItemSlotID].id) == originalItemFromSelectedSlot.id)
        {
            if ((slotType == "slot" ? Inventory.items[selectedItemSlotID].quantity : Inventory.hotbarItems[selectedItemSlotID].quantity) + grabbedItemQuantity <= quantityLimitPerSlot && grabbedItemSlotID != selectedItemSlotID)
            {
                if (slotType == "slot")
                    Inventory.items[selectedItemSlotID].quantity += grabbedItemQuantity;
                else
                    Inventory.hotbarItems[selectedItemSlotID].quantity += grabbedItemQuantity;

                if (!grabbedItemSlotInHotbar)
                    Inventory.items[grabbedItemSlotID] = null;
                else
                    Inventory.hotbarItems[grabbedItemSlotID] = null;

                grabbedItemQuantity = 0;

            }
            else if (originalItemFromSelectedSlot.quantity < quantityLimitPerSlot && grabbedItemSlotID != selectedItemSlotID)
            {
                grabbedItemQuantity -= quantityLimitPerSlot - (slotType == "slot" ? Inventory.items[selectedItemSlotID].quantity : Inventory.hotbarItems[selectedItemSlotID].quantity);

                if (slotType == "slot")
                    Inventory.items[selectedItemSlotID].quantity = quantityLimitPerSlot;
                else
                    Inventory.hotbarItems[selectedItemSlotID].quantity = quantityLimitPerSlot;

                if (!grabbedItemSlotInHotbar)
                    Inventory.items[grabbedItemSlotID].quantity = grabbedItemQuantity;
                else
                    Inventory.hotbarItems[grabbedItemSlotID].quantity = grabbedItemQuantity;

                replaceItems = false;
            }
            else
            {
                replaceItems = true;
            }
        }
        else
        {
            if (slotType == "slot")
            {
                Inventory.items[selectedItemSlotID] = grabbedItemSlotInHotbar ? Inventory.hotbarItems[grabbedItemSlotID] : Inventory.items[grabbedItemSlotID];
                if (!grabbedItemSlotInHotbar)
                    Inventory.items[grabbedItemSlotID] = originalItemFromSelectedSlot;
                else
                    Inventory.hotbarItems[grabbedItemSlotID] = originalItemFromSelectedSlot;
            }
            else
            {
                Inventory.hotbarItems[selectedItemSlotID] = grabbedItemSlotInHotbar ? Inventory.hotbarItems[grabbedItemSlotID] : Inventory.items[grabbedItemSlotID];
                if (!grabbedItemSlotInHotbar)
                    Inventory.items[grabbedItemSlotID] = originalItemFromSelectedSlot;
                else
                    Inventory.hotbarItems[grabbedItemSlotID] = originalItemFromSelectedSlot;
            }
            replaceItems = false;
        }


        if (replaceItems)
        {
            if (!grabbedItemSlotInHotbar)
            {
                Inventory.items[selectedItemSlotID] = Inventory.items[grabbedItemSlotID];
                Inventory.items[grabbedItemSlotID] = originalItemFromSelectedSlot;
            }
            else
            {
                Inventory.hotbarItems[selectedItemSlotID] = Inventory.hotbarItems[grabbedItemSlotID];
                Inventory.hotbarItems[grabbedItemSlotID] = originalItemFromSelectedSlot;
            }
        }
        grabbedItemSlotID = -1;
        grabbedItem = null;
        grabbedItemSlotInHotbar = false;
        cameraObject.GetComponent<Inventory>().ReloadInventory();
    }
}
