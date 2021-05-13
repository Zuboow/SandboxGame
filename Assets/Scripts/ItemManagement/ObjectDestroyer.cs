using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) && !Inventory.inventoryOpened && Input.GetMouseButtonDown(0))
        {
            if (hit.transform.gameObject.tag == "Destroyable" && Vector3.Distance(hit.point, transform.position) < 2f)
            {
                if (hit.transform.gameObject.GetComponent<ItemDropper>().DropItems() == true)
                {
                    Destroy(hit.transform.gameObject);
                    Debug.Log("Items dropped");
                }
            }
        }
    }
}
