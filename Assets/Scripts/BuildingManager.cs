using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public List<GameObject> objectBuilders;
    bool buildingMode = false;
    Vector3 buildingTarget;
    GameObject spawner;
    float rotationOffset = 0f;
    void Start()
    {
        spawner = Instantiate(objectBuilders[0]);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            buildingMode = !buildingMode;
        }

        if (buildingMode)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode))) //manage pressed keys
            {
                if (Input.GetKeyDown(kcode))
                    ManageKeys(kcode);
            }

            if (Physics.Raycast(ray, out hit) && !Inventory.inventoryOpened)
            {
                if ((hit.transform.gameObject.tag == "Buildable" || hit.transform.gameObject.tag == "Terrain") && Vector3.Distance(hit.point, transform.position) < 3f)
                {
                    if (hit.collider.tag != "Buildable") //don't stick spawner
                    {
                        spawner.SetActive(true);
                        spawner.transform.position = hit.point;
                        spawner.transform.rotation = Quaternion.Euler(0, rotationOffset, 0);
                        buildingTarget = hit.point;
                    }
                    else if (hit.collider.gameObject.GetComponent<RotationChecker>() != null) //stick spawner to existing object
                    {
                        spawner.transform.rotation = Quaternion.Euler(0, rotationOffset, 0);
                        spawner.SetActive(true);
                        if (!spawner.transform.Find("collider").GetComponent<RotationChecker>().rotated) //not rotated objects are for example pillars
                        {
                            if (!hit.collider.gameObject.GetComponent<RotationChecker>().rotated) //stick to not rotated objects
                            {
                                if (Vector3.Distance(hit.collider.ClosestPointOnBounds(hit.point), hit.collider.bounds.min) //stick to top
                                        > Vector3.Distance(hit.collider.ClosestPointOnBounds(hit.point), hit.collider.bounds.max))
                                {
                                    spawner.transform.position = hit.collider.bounds.center + new Vector3(0, hit.collider.bounds.extents.y, 0);
                                }
                                else //stick to bottom
                                {
                                    spawner.transform.position = hit.collider.bounds.center - new Vector3(0, hit.collider.bounds.extents.y * 3, 0);
                                }
                            }
                            else //stick to rotated objects
                            {
                                if (Vector3.Distance(hit.collider.ClosestPointOnBounds(hit.point), hit.collider.bounds.min)
                                        < Vector3.Distance(hit.collider.ClosestPointOnBounds(hit.point), hit.collider.bounds.max)) //stick to left side
                                {
                                    spawner.transform.position =
                                        new Vector3(hit.collider.gameObject.GetComponent<RotationChecker>().offset == 0 ? hit.collider.bounds.min.x
                                        : hit.collider.gameObject.GetComponent<RotationChecker>().offset == 180 ? hit.collider.bounds.min.x
                                        : hit.collider.bounds.center.x, hit.collider.gameObject.transform.position.y, hit.collider.gameObject.GetComponent<RotationChecker>().offset == 90 ? hit.collider.bounds.min.z
                                        : hit.collider.gameObject.GetComponent<RotationChecker>().offset == 270 ? hit.collider.bounds.min.z
                                        : hit.collider.bounds.center.z);
                                }
                                else //stick to right side
                                {
                                    spawner.transform.position =
                                        new Vector3(hit.collider.gameObject.GetComponent<RotationChecker>().offset == 0 ? hit.collider.bounds.max.x
                                        : hit.collider.gameObject.GetComponent<RotationChecker>().offset == 180 ? hit.collider.bounds.max.x
                                        : hit.collider.bounds.center.x, hit.collider.gameObject.transform.position.y, hit.collider.gameObject.GetComponent<RotationChecker>().offset == 90 ? hit.collider.bounds.max.z
                                        : hit.collider.gameObject.GetComponent<RotationChecker>().offset == 270 ? hit.collider.bounds.max.z
                                        : hit.collider.bounds.center.z);
                                }
                            }
                        }
                        else //rotated objects are objects like walls, fences, etc.
                        {
                            if (!hit.collider.gameObject.GetComponent<RotationChecker>().rotated) //touches not rotated
                            {
                                spawner.transform.position = hit.collider.bounds.center - new Vector3(0, hit.collider.bounds.extents.y, 0);
                            }
                            else //touches rotated
                            {
                                spawner.transform.position = hit.collider.bounds.center
                                + new Vector3(hit.collider.gameObject.GetComponent<RotationChecker>().offset == 0 ? hit.collider.bounds.extents.x
                                : hit.collider.gameObject.GetComponent<RotationChecker>().offset == 180 ? -hit.collider.bounds.extents.x
                                : 0, 0, hit.collider.gameObject.GetComponent<RotationChecker>().offset == 90 ? -hit.collider.bounds.extents.z
                                : hit.collider.gameObject.GetComponent<RotationChecker>().offset == 270 ? hit.collider.bounds.extents.z
                                : 0);
                            }
                        }
                        buildingTarget = spawner.transform.position;
                    }
                    else
                    {
                        spawner.SetActive(false);
                    }

                    if (!Inventory.inventoryOpened)
                    {
                        if (Input.GetMouseButtonDown(0)) //spawn object
                        {
                            BuildObject(buildingTarget, hit.collider.gameObject.transform.parent);
                        }
                        if (Input.GetMouseButtonDown(1) && hit.transform.gameObject.tag == "Buildable") //remove object
                        {
                            if (hit.collider.transform.parent != null)
                                Destroy(hit.collider.transform.parent.gameObject);
                            else
                                Destroy(hit.collider.gameObject);
                        }
                    }

                }
                else
                {
                    spawner.SetActive(false);
                }
            }
            else
            {
                spawner.SetActive(false);
            }
        }
        else
        {
            spawner.SetActive(false);
        }
    }

    void BuildObject(Vector3 target, Transform parent)
    {
        GameObject spawnedBuildable = Instantiate(spawner, target, spawner.transform.rotation);
        spawnedBuildable.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/" + "OrangeTexture");
        if (spawnedBuildable.GetComponent<BoxCollider>() != null)
            spawnedBuildable.GetComponent<BoxCollider>().isTrigger = false;
        else
            spawnedBuildable.GetComponent<MeshCollider>().isTrigger = false;

        if (spawnedBuildable.name.Split('_')[1].Split('(')[0].Trim() == "raycastable")
            spawnedBuildable.layer = 0;
        spawnedBuildable.transform.Find("collider").GetComponent<BoxCollider>().isTrigger = false;
        spawnedBuildable.transform.Find("collider").gameObject.tag = "Buildable";
        spawnedBuildable.transform.Find("collider").gameObject.layer = 0;
        spawnedBuildable.transform.Find("collider").gameObject.name = spawner.name;
        spawnedBuildable.name = spawner.name;
        spawner.transform.rotation = Quaternion.Euler(0, rotationOffset, 0);
    }

    void ManageKeys(KeyCode key)
    {
        if (key == KeyCode.Alpha1)
        {
            Destroy(spawner);
            spawner = Instantiate(objectBuilders[0]);
            spawner.transform.Find("collider").GetComponent<RotationChecker>().offset = rotationOffset;
            spawner.SetActive(false);
        }
        else if (key == KeyCode.Alpha2)
        {
            Destroy(spawner);
            spawner = Instantiate(objectBuilders[1]);
            spawner.transform.Find("collider").GetComponent<RotationChecker>().offset = rotationOffset;
            spawner.SetActive(false);
        }
        else if (key == KeyCode.Alpha3)
        {
            Destroy(spawner);
            spawner = Instantiate(objectBuilders[2]);
            spawner.transform.Find("collider").GetComponent<RotationChecker>().offset = rotationOffset;
            spawner.SetActive(false);
        }
        else if (key == KeyCode.E)
        {
            spawner.transform.Rotate(Vector3.up, 90, 0);
            rotationOffset += 90;
            if (rotationOffset == 360)
                rotationOffset = 0;
            spawner.transform.Find("collider").GetComponent<RotationChecker>().offset = rotationOffset;
        }
        else if (key == KeyCode.Q)
        {
            spawner.transform.Rotate(Vector3.up, -90, 0);
            rotationOffset -= 90;
            if (rotationOffset == -90)
                rotationOffset = 270;
            spawner.transform.Find("collider").GetComponent<RotationChecker>().offset = rotationOffset;
        }
    }
}
