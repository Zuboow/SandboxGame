using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    public float sensitivity = 100f;

    void Start()
    {
        
    }

    void Update()
    {
        //float yAxis = Input.GetAxis("Controller Y");
        //float xAxis = Input.GetAxis("Controller X");
        float yAxis = Input.GetAxis("Mouse Y");
        float xAxis = Input.GetAxis("Mouse X");

        if (!Inventory.inventoryOpened)
        {
            transform.RotateAround(player.transform.position, -Vector3.up, (-xAxis * sensitivity));
            transform.RotateAround(player.transform.position, transform.right, (-yAxis * sensitivity));
        }
    }
}
