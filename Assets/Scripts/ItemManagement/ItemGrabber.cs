using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrabber : MonoBehaviour
{
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !Inventory.inventoryOpened)
        {
            foreach (Item i in Inventory.itemsFromJSON.items)
            {
                Debug.Log(name.Split('(')[0].Trim());
                if (name.Split('(')[0].Trim() == i.spriteName)
                {
                    SlotManager.AddItem(i.id, 1);
                    Destroy(gameObject);
                }
            }
        }
    }
}
