using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // reference the transform
    Transform t;

    public static bool inWater;
    public static bool isSwiming;
    // if not in water -> walk
    // if in water and not swimming -> float
    // if in water and swimming -> swim

    public LayerMask waterMask;

    [Header("Player Rotation")]
    public float sensitivity = 1;

    // Clamp Variable
    public float rotationMin, rotationMax;

    //mouse input variables
    float rotationX;
    float rotationY;

    [Header("Player Movement")]
    public float speed = 1;
    float moveX, moveY, moveZ;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        t = this.transform;

        // Disapear the cursor and lock it to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;

        inWater = false;
        rb.useGravity = true;
    }

    private void FixedUpdate()
    {
        SwimmingOrFloating();
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        SwitchMovement();
    }
    private void OnTriggerExit(Collider other)
    {
        SwitchMovement();
    }

    void SwitchMovement()
    {
        // toggle inWater
        inWater = !inWater;

        // toggle the gravity of the rigidbody
        rb.useGravity = !rb.useGravity;
    }

    void SwimmingOrFloating()
    {
        bool swimCheck = false;
        if ((inWater))
        {
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(t.position.x, t.position.y + 0.5f, t.position.z), Vector3.down, out hit, Mathf.Infinity, waterMask))
            {
                if (hit.distance < 0.1f)
                {
                    swimCheck = true;
                }
            }
            else swimCheck = true;
        }

        isSwiming = swimCheck;
        Debug.Log("isSwiming = " + isSwiming);
    }

    // Update is called once per frame
    void Update()
    {
        LookAround();

        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void LookAround()
    {
        // get the mouse input
        rotationX += Input.GetAxis("Mouse X") * sensitivity;
        rotationY += Input.GetAxis("Mouse Y") * sensitivity;

        // Clamp the Y rotation
        rotationY = Mathf.Clamp(rotationY, rotationMin, rotationMax);

        // Setting the rotation value every update
        t.localRotation = Quaternion.Euler(-rotationY, rotationX, 0);
    }

    void Move()
    {
        // get the movement input
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        moveZ = Input.GetAxis("Forward");



        if (!inWater)
        {
            // check if the player is standing still
            if (moveX == 0 && moveZ == 0)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            // Move the character (Land ver)
            t.Translate(new Quaternion(0, t.rotation.y, 0, t.rotation.w) * new Vector3(moveX, 0, moveZ) * Time.deltaTime * speed, Space.World);
        }
        else
        {
            rb.velocity = Vector2.zero;

            // Check if the player is swimming under water or floating along the top
            if (!isSwiming)
            {
                // Move the player (Floating ver)
                // Clamp the moveY value, so the player cannot use space or shift to move up
                moveY = Mathf.Min(moveY, 0);

                // convert the local direction vector into a worldspace vector
                Vector3 clampedDirection = t.TransformDirection(new Vector3(moveX, moveY, moveZ));

                // Clamp the value of the worldspace vector
                clampedDirection = new Vector3(clampedDirection.x, Mathf.Min(clampedDirection.y, 0), clampedDirection.z);

                t.Translate(clampedDirection * Time.deltaTime * speed, Space.World);
            }
            else
            {
                // move the caracter (swimming ver)
                t.Translate(new Vector3(moveX, 0, moveZ) * Time.deltaTime * speed);
                t.Translate(new Vector3(0, moveY, 0) * Time.deltaTime * speed, Space.World);
            }
        }

    }

}
