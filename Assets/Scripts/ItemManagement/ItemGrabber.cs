using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrabber : MonoBehaviour
{
    void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.E) && !Inventory.inventoryOpened)
        {
            foreach (Item i in Inventory.itemsFromJSON.items)
            {
                if (name.Split('(')[0].Trim() == i.spriteName)
                {
                    if (SlotManager.AddItem(i.id, 1))
                        Destroy(gameObject);
                }
            }
        }
    }
}
