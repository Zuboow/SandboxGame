using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public GameObject particleSpawner;
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) && !Inventory.inventoryOpened && Input.GetMouseButtonDown(0) && !BuildingManager.buildingMode && HealthManager.playerAlive)
        {
            if (hit.transform.gameObject.tag == "Destroyable" && Vector3.Distance(hit.point, transform.position) < 2f)
            {
                GameObject particles;
                particles = Instantiate(particleSpawner, hit.point, Quaternion.identity);
                particles.GetComponent<ParticleSystem>().Play();
                if (hit.transform.gameObject.GetComponent<EntityHealthManager>().DamageEntity(5)) //constant damage for testing purposes
                {
                    hit.transform.gameObject.GetComponent<ItemDropper>().DropItems();
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }
}
