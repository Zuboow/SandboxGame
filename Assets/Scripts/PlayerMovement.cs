using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 4f;
    public GameObject cameraObject;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float yAxis = Input.GetAxis("Vertical");
        float xAxis = Input.GetAxis("Horizontal");

        if ((yAxis != 0 || xAxis != 0) && HealthManager.playerAlive)
        {
            Vector3 direction = Camera.main.transform.forward * yAxis + Camera.main.transform.right * xAxis;
            //transform.position = Vector3.MoveTowards(transform.position ,transform.position + direction, speed * 0.2f);
            GetComponent<Rigidbody>().MovePosition(transform.position + direction * speed * 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0), speed);
        }

    }
}
