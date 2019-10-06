using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rbody;
    private Animator playerAnim;
    public GameObject FirstPersonCam;
    public GameObject ThirdPersonCam;

    //Variables for Players 3rd Person Camera
    public Transform playerCamera;
    public float turnSmoothing = 0.06f;
    public float springFOV = 100f;
    public string sprintButton = "Sprint";

    //Movement Variables
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float sprintSpeed = 12f;
    public float speedDampTime = 0.1f;
    public float rotSpeed = 100f;

    private int hFloat;
    private int vFloat;

    //Jump Variables
    public string jumpButton = "Jump";
    public float jumpHeight = 5.0f;
    public float jumpInterialForce = 10f;

    private float speed, speedSeeker;
    private int jumpBool;
    private int groundedBool;
    private int speedFloat;
    private Vector3 colExtents;
    public bool canJump;

    private bool jump;
    private bool isColliding;

    public bool FpsMovement = true;
    public bool ThrdPersonMovement = false;

    private Vector3 lastDirection;


    // Start is called before the first frame update
    void Start()
    {

        rbody = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();

        FpsMovement = true;
        ThrdPersonMovement = false;

        jumpBool = Animator.StringToHash("Jump");
        groundedBool = Animator.StringToHash("Grounded");
        playerAnim.SetBool(groundedBool, true);
        speedFloat = Animator.StringToHash("Speed");

        hFloat = Animator.StringToHash("H");
        vFloat = Animator.StringToHash("V");


    }

    // Update is called once per frame
    void Update()
    {

        PerspectiveSwitcher();
        JumpManager();

        speedSeeker += Input.GetAxis("Mouse ScrollWheel");
        speedSeeker = Mathf.Clamp(speedSeeker, walkSpeed, runSpeed);
        speed = speedSeeker;

        playerAnim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);

        // Set the input axes on the Animator Controller.
        playerAnim.SetFloat(hFloat, Input.GetAxis("Horizontal"), 0.1f, Time.deltaTime);
        playerAnim.SetFloat(vFloat, Input.GetAxis("Vertical"), 0.1f, Time.deltaTime);

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

            if(Input.GetKey(KeyCode.Space) && canJump == true)
            {

                rbody.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);

            }
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

    void ThirdPersonManager()
    {

        Vector3 forward = playerCamera.TransformDirection(Vector3.forward);
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        Vector3 targetDirection;
        targetDirection = forward + right;

        if (Input.GetKey(KeyCode.LeftShift))
        {

            rbody.velocity = (Input.GetAxis("Horizontal") * right * sprintSpeed) + (Input.GetAxis("Vertical") * forward * sprintSpeed) + new Vector3(0, rbody.velocity.y, 0);

        }
        else
        {

            rbody.velocity = (Input.GetAxis("Horizontal") * right * speedSeeker) + (Input.GetAxis("Vertical") * forward * speedSeeker) + new Vector3(0, rbody.velocity.y, 0);

        }

        

        if(Input.GetAxis("Horizontal") !=0 || Input.GetAxis("Vertical") !=0 && targetDirection != Vector3.zero)
        {

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            Quaternion newRotation = Quaternion.Slerp(rbody.rotation, targetRotation, turnSmoothing);
            rbody.MoveRotation(newRotation);


        }

        if (lastDirection != Vector3.zero)
        {
            lastDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(lastDirection);
            Quaternion newRotation = Quaternion.Slerp(rbody.rotation, targetRotation, turnSmoothing);
            rbody.MoveRotation(newRotation);
        }
    }

    public void PerspectiveSwitcher()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {

            FpsMovement = !FpsMovement;
            ThrdPersonMovement = !ThrdPersonMovement;

        }

        if(FpsMovement == true)
        {

            FirstPersonCam.SetActive(true);
            ThirdPersonCam.SetActive(false);

            FirstPersonManager();

        }

        if(ThrdPersonMovement == true)
        {

            FirstPersonCam.SetActive(false);
            ThirdPersonCam.SetActive(true);

            ThirdPersonManager();

        }
    }

    private void RemoveVerticalVelocity()
    {
        Vector3 horizontalVelocity = rbody.velocity;
        horizontalVelocity.y = 0;
        rbody.velocity = horizontalVelocity;
    }

    // Get the last player direction of facing.
    public Vector3 GetLastDirection()
    {
        return lastDirection;
    }

    // Set the last player direction of facing.
    public void SetLastDirection(Vector3 direction)
    {
        lastDirection = direction;
    }

    public bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 2 * colExtents.x, Vector3.down);
        return Physics.SphereCast(ray, colExtents.x, colExtents.x + 0.2f);
    }
}
