using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrabber : MonoBehaviour
{
    void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.E) && !Inventory.inventoryOpened && HealthManager.playerAlive)
        {
            foreach (Item i in Inventory.itemsFromJSON.items)
            {
                if (name.Split('(')[0].Trim() == i.spriteName)
                {
                    if (SlotManager.AddItem(i.id, 1) == 0)
                    {
                        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Inventory>().ReloadHotbar();
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
