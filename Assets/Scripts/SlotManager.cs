using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotManager : MonoBehaviour
{
    GameObject cameraObject;

    void Start()
    {
        cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Removing item...");
            RemoveItem(Int32.Parse(name.Split('_')[1]));
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
}
