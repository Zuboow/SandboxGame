using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public List<GameObject> objectBuilders;
    List<GameObject> anchors = new List<GameObject>();
    bool buildingMode = false, pivoted = false, rotated = false;
    string pivotSide = "";
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

            for (int x = 0; x < anchors.Count; x++) //hides all anchors
            {
                if (anchors[x] == null)
                {
                    anchors.RemoveAt(x);
                } 
                else
                {
                    if (buildingMode)
                        anchors[x].SetActive(true);
                    else
                        anchors[x].SetActive(false);
                }
            }
        }

        if (buildingMode)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if ((hit.transform.gameObject.tag == "Buildable" || hit.transform.gameObject.tag == "Terrain" || hit.transform.gameObject.tag == "Anchor")
                    && Vector3.Distance(hit.point, transform.position) < 2f)
                {
                    if (hit.collider.name == "leftPivot" || hit.collider.name == "rightPivot") //sticks builder to anchor
                    {
                        spawner.SetActive(true);
                        spawner.transform.position = hit.collider.gameObject.transform.position;
                        spawner.gameObject.transform.Find("mesh").GetComponent<MeshFilter>().mesh = Resources.Load<GameObject>("Prefabs/Buildables/" +
                            spawner.name.Split('(')[0].Trim() + "_pivotL").transform.Find("mesh").GetComponent<MeshFilter>().sharedMesh;
                        if (hit.collider.name == "leftPivot")
                        {
                            if (!hit.collider.gameObject.transform.parent.gameObject.GetComponent<RotationChecker>().rotated)
                            {
                                rotated = true;
                                spawner.transform.rotation = Quaternion.Euler(0, rotationOffset - 180, 0);
                            }
                            else
                                spawner.transform.rotation = Quaternion.Euler(0, rotationOffset - 180, 0);
                        }
                        if (hit.collider.name == "rightPivot")
                        {
                            if (!hit.collider.gameObject.transform.parent.gameObject.GetComponent<RotationChecker>().rotated)
                            {
                                rotated = false;
                                spawner.transform.rotation = Quaternion.Euler(0, rotationOffset, 0);
                            }
                            else 
                                spawner.transform.rotation = Quaternion.Euler(0, rotationOffset, 0);
                        }
                        pivoted = true;
                        pivotSide = hit.collider.gameObject.GetComponent<MeshFilter>().mesh.name == "leftPivot" ? "right" : "left";
                        buildingTarget = hit.collider.gameObject.transform.position;
                    }
                    else //building without sticking to objects
                    {
                        spawner.SetActive(true);
                        spawner.transform.position = hit.point;
                        spawner.transform.rotation = Quaternion.Euler(0, rotationOffset, 0);
                        rotated = false;
                        spawner.gameObject.transform.Find("mesh").GetComponent<MeshFilter>().mesh = Resources.Load<GameObject>("Prefabs/Buildables/" +
                            spawner.name.Split('(')[0].Trim() + "_straight").transform.Find("mesh").GetComponent<MeshFilter>().sharedMesh;
                        pivoted = false;
                        buildingTarget = hit.point;
                    }


                    foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode))) //manages pressed keys
                    {
                        if (Input.GetKeyDown(kcode))
                            ManageKeys(kcode);
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        BuildObject(buildingTarget, hit.collider.gameObject.transform.parent);
                    }
                    if (Input.GetMouseButtonDown(1) && hit.transform.gameObject.tag == "Buildable")
                    {
                        Destroy(hit.collider.gameObject.transform.parent.gameObject);
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
        GameObject spawnedBuildable = Instantiate(Resources.Load<GameObject>("Prefabs/Buildables/" + spawner.name.Split('(')[0].Trim() + (!pivoted ? "_straight" : "_pivotL")), 
            target, spawner.transform.rotation);
        if ((rotated && spawnedBuildable.tag != "NoRotation") || (!rotated && spawnedBuildable.tag == "NoRotation"))
        {
            spawnedBuildable.GetComponent<RotationChecker>().rotated = true;
            if (spawnedBuildable.tag == "NoRotation")
            {
                var lPivot = spawnedBuildable.transform.Find("leftPivot").gameObject;
                var rPivot = spawnedBuildable.transform.Find("rightPivot").gameObject;
                lPivot.name = "rightPivot";
                rPivot.name = "leftPivot";
            }
        }
        anchors.AddRange(GameObject.FindGameObjectsWithTag("Anchor"));
        spawner.transform.rotation = Quaternion.Euler(0, rotationOffset, 0);
    }

    void ManageKeys(KeyCode key)
    {
        if (key == KeyCode.Alpha1)
        {
            Destroy(spawner);
            spawner = Instantiate(objectBuilders[0]);
            spawner.SetActive(false);
        }
        else if (key == KeyCode.Alpha2)
        {
            Destroy(spawner);
            spawner = Instantiate(objectBuilders[1]);
            spawner.SetActive(false);
        }
        else if (key == KeyCode.Alpha3)
        {
            Destroy(spawner);
            spawner = Instantiate(objectBuilders[2]);
            spawner.SetActive(false);
        }
        else if (key == KeyCode.Alpha4)
        {
            Destroy(spawner);
            spawner = Instantiate(objectBuilders[3]);
            spawner.SetActive(false);
        }
        else if (key == KeyCode.Alpha5)
        {
            Destroy(spawner);
            spawner = Instantiate(objectBuilders[4]);
            spawner.SetActive(false);
        }
        else if (key == KeyCode.E)
        {
            spawner.transform.Rotate(Vector3.up, 30, 0);
            rotationOffset += 30;
        }
        else if (key == KeyCode.Q)
        {
            spawner.transform.Rotate(Vector3.up, -30, 0);
            rotationOffset -= 30;
        }
    }
}
