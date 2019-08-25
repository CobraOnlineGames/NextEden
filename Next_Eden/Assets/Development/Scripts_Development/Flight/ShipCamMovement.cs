using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCamMovement : MonoBehaviour
{

    public GameObject player;

    public float rotSpeed;

    public float yRot;

    public const float yAngleMin = -45.0f;
    public const float yAngleMax = 80.0f;


    // Start is called before the first frame update
    void Start()
    {

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //Flight Cam follows the players position
        transform.position = player.transform.position;

        //Rotate with Mouse X with 3rd Person Cam
        transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime, Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime, 0));

    }
}
