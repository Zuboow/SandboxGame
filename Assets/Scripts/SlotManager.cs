using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    GameObject cameraObject;
    static GameObject grabbedObject;
    public static int grabbedItemSlotID = -1;
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

    public void ManageMouseInput(string name)
    {
        if (Input.GetMouseButtonDown(1) && grabbedItemSlotID == -1)
        {
            RemoveItem(Int32.Parse(name));
        }
        if (Input.GetMouseButtonDown(0) && grabbedItemSlotID == -1)
        {
            GrabItem(Int32.Parse(name));
        }
        else if (Input.GetMouseButtonDown(0) && grabbedItemSlotID != -1)
        {
            ManageMovingObjects(Int32.Parse(name));
        }
    }

    public static bool AddItem(int id)
    {
        foreach (KeyValuePair<int, int> key in Inventory.items)
        {
            if (key.Value == -1)
            {
                Inventory.items[key.Key] = id;
                if (Inventory.inventoryOpened)
                    GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Inventory>().ReloadInventory();

                return true;
            }
        }
        return false;
    }


    public void RemoveItem(int slotID)
    {
        if (Inventory.items[slotID] != -1)
        {
            //testing purposes
            Instantiate(Resources.Load<GameObject>("Prefabs/ObjectsDropped/" + "block01_"), GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0f,0.5f,0f), Quaternion.identity); 
            Inventory.items[slotID] = -1;
            cameraObject.GetComponent<Inventory>().ReloadInventory();
        }
    }

    void GrabItem(int slotID)
    {
        if (Inventory.items[slotID] != -1)
        {
            grabbedObject = transform.Find("spriteSpawner(Clone)").gameObject;
            transform.SetAsLastSibling();
            grabbedItemSlotID = slotID;
        }
    }

    static void MoveGrabbedItem(GameObject grabbedObjectInstance)
    {
        grabbedObjectInstance.transform.position = Input.mousePosition;
    }

    void ManageMovingObjects(int selectedItemSlotID)
    {
        int originalIDFromSelectedSlot = Inventory.items[selectedItemSlotID];
        Inventory.items[selectedItemSlotID] = Inventory.items[grabbedItemSlotID];
        Inventory.items[grabbedItemSlotID] = originalIDFromSelectedSlot;
        grabbedItemSlotID = -1;
        cameraObject.GetComponent<Inventory>().ReloadInventory();
    }
}
