using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;     // Walking speed
    public float jumpHeight = 2f;    // Jump height
    public float gravity = -9.8f;    // Gravity strength

    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 100f;  // Mouse sensitivity for look-around
    public Camera playerCamera;            // Player's camera

    [Header("Ground Detection")]
    public Transform groundCheck;          // Ground check object
    public LayerMask groundMask;           // Layer mask to identify ground
    public float groundCheckRadius = 0.3f; // Radius for ground detection

    private CharacterController controller;   // CharacterController for movement
    private Vector3 velocity;                  // Player velocity
    private bool isGrounded;                   // Grounded status
    private float xRotation = 0f;              // Vertical rotation of the camera

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Unlock the cursor
        Cursor.visible = false;                 // Make the cursor visible
    }

    void Update()
    {
        HandleMouseLook();  // Handle mouse look-around
        HandleMovement();   // Handle player movement
    }

    void HandleMouseLook()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Adjust camera's vertical rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical rotation to prevent flipping

        // Rotate the camera and player
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        // Check if grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        // If grounded and falling, reset y-velocity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift)) {
            moveSpeed += 8;
        }

        if(Input.GetKeyUp(KeyCode.LeftShift)) {
            moveSpeed -= 8;
        }

        // Get movement input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Create movement direction
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Move the player
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the ground check radius in the Scene view
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
