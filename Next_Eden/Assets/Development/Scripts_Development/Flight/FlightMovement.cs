using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightMovement : MonoBehaviour
{

    public float flightSpeed;

    private Rigidbody rbody;

    public GameObject camPosition;

    // Start is called before the first frame update
    void Start()
    {

        rbody = gameObject.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {

        FlightControl();

    }

    public void FlightControl()
    {

        if (Input.GetKey(KeyCode.W))
        {

            transform.Translate(0, 0, flightSpeed * Time.deltaTime);
            transform.eulerAngles = camPosition.transform.eulerAngles;

        }

        if (Input.GetKey(KeyCode.S))
        {

            transform.Translate(0, 0, -flightSpeed * Time.deltaTime);
            transform.eulerAngles = camPosition.transform.eulerAngles;

        }

        if (Input.GetKey(KeyCode.A))
        {

            transform.Translate(-flightSpeed * Time.deltaTime, 0, 0);
            transform.eulerAngles = camPosition.transform.eulerAngles;

        }

        if (Input.GetKey(KeyCode.D))
        {

            transform.Translate(flightSpeed * Time.deltaTime, 0, 0);
            transform.eulerAngles = camPosition.transform.eulerAngles;

        }
        //Ship Movement
        // Vector3 forward = gameObject.transform.forward;
        // Vector3 right = gameObject.transform.right;

        // rbody.velocity = (Input.GetAxis("Horizontal") * right * flightSpeed) + (Input.GetAxis("Vertical") * forward * flightSpeed) + new Vector3(0, rbody.velocity.y, 0);

        //Ship Rotation via Camera

       // Vector3 rotation = gameObject.transform.eulerAngles;
       // Vector3 camRotation = camPosition.transform.eulerAngles;

        //gameObject.transform.eulerAngles = new Vector3(camRotation.x, camRotation.y, camRotation.z);


    }
}
