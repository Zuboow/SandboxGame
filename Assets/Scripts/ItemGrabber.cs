using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrabber : MonoBehaviour
{
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !Inventory.inventoryOpened)
        {
            switch (name.Split('(')[0].Trim())
            {
                //testing purposes
                case "block01_":
                    if (SlotManager.AddItem(1) == true)
                        Destroy(gameObject);
                    break;
            }
        }
    }
}
