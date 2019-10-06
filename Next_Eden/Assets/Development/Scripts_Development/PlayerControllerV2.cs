using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerV2 : MonoBehaviour
{
    //Needed Assets and Components
    public Transform playerCamera;
    public GameObject firstCam;
    public GameObject thirdCam;
    private Rigidbody rbody;
    private Animator playerAnim;

    //Needed Variables for Movement
    public float walkSpeed = 3.0f;
    public float jogSpeed = 6.0f;
    public float sprintSpeed = 10.0f;
    public float speedDampTime = 0.1f;
    public float turnSmoothing = 10.0f;
    public float rotSpeed = 100.0f;
    private int groundedBool;

    private int hFloat;
    private int vFloat;

    //Needed Variables for Jumping
    private string jumpButton = "Jump";
    public float jumpHeight = 2.0f;
    public float jumpInterialForce = 10f;
    private int jumpBool;
    private bool canJump;

    private float speed, speedSeeker;

    //Perspective Switcher Variables
    private bool FirstPerspective;
    private bool ThirdPerspective;
    


    // Start is called before the first frame update
    void Start()
    {
        //Referencing Components
        rbody = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();

        //Referencing Variables
        jumpBool = Animator.StringToHash("Jump");
        groundedBool = Animator.StringToHash("Grounded");
        playerAnim.SetBool(groundedBool, true);

        FirstPerspective = true;
        ThirdPerspective = false;

    }

    // Update is called once per frame
    void Update()
    {

        speedSeeker += Input.GetAxis("Mouse ScrollWheel");
        speedSeeker = Mathf.Clamp(speedSeeker, walkSpeed, jogSpeed);
        speed *= speedSeeker;

        playerAnim.SetFloat("speed", speedSeeker);

        PerspectiveSwitcher();
        JumpManager();
        
    }

    public void AnimatorManager()
    {



    }

    public void JumpManager()
    {


        Ray jumpRay = new Ray();

        jumpRay.origin = transform.position;
        jumpRay.direction = transform.up * -1.0f;

        Debug.DrawLine(jumpRay.origin, jumpRay.origin + jumpRay.direction * 0.3f, Color.blue);

        RaycastHit jumpRayhit;
        if (Physics.Raycast(jumpRay, out jumpRayhit, 0.3f))
        {

            if (jumpRayhit.collider.tag == "Floor")
            {

                canJump = true;

            }
            else
            {

                canJump = false;

            }

            if (Input.GetKey(KeyCode.Space) && canJump == true)
            {

                rbody.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);

            }
        }

    }

    public void ThirdPersonManager()
    {

        Vector3 forward = playerCamera.TransformDirection(Vector3.forward);
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        Vector3 targetDirection;
        targetDirection = forward;

        if (Input.GetKey(KeyCode.LeftShift))
        {

            rbody.velocity = (Input.GetAxis("Horizontal") * right * sprintSpeed) + (Input.GetAxis("Vertical") * forward * sprintSpeed) + new Vector3(0, rbody.velocity.y, 0);

        }
        else
        {

            rbody.velocity = (Input.GetAxis("Horizontal") * right * speedSeeker) + (Input.GetAxis("Vertical") * forward * speedSeeker) + new Vector3(0, rbody.velocity.y, 0);

        }

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 && targetDirection != Vector3.zero)
        {

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            Quaternion newRotation = Quaternion.Slerp(rbody.rotation, targetRotation, turnSmoothing);
            rbody.MoveRotation(newRotation);


        }

    }

    public void FirstPersonManager()
    {

        Vector3 forward = gameObject.transform.forward;
        Vector3 right = gameObject.transform.right;

        if (Input.GetKey(KeyCode.LeftShift))
        {

            rbody.velocity = (Input.GetAxis("Horizontal") * right * sprintSpeed) + (Input.GetAxis("Vertical") * forward * sprintSpeed) + new Vector3(0, rbody.velocity.y, 0);

        }
        else
        {

            rbody.velocity = (Input.GetAxis("Horizontal") * right * speedSeeker) + (Input.GetAxis("Vertical") * forward * speedSeeker) + new Vector3(0, rbody.velocity.y, 0);

        }

        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime, 0));

    }

    public void PerspectiveSwitcher()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {

            FirstPerspective = !FirstPerspective;
            ThirdPerspective = !ThirdPerspective;

        }

        if (FirstPerspective == true)
        {

            firstCam.SetActive(true);
            thirdCam.SetActive(false);

            FirstPersonManager();

        }

        if (ThirdPerspective == true)
        {

            firstCam.SetActive(false);
            thirdCam.SetActive(true);

            ThirdPersonManager();

        }


    }
}
