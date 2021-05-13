using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public List<int> droppedItems;

    public bool DropItems()
    {
        foreach (int itemID in droppedItems)
        {
            foreach (Item i in Inventory.itemsFromJSON.items)
            {
                if (itemID == i.id)
                {
                    Instantiate(Resources.Load<GameObject>("Prefabs/ObjectsDropped/" + i.prefabName), transform.position + new Vector3(0,1,0), Quaternion.identity);
                }
            }
        }
        return true;
    }
}
