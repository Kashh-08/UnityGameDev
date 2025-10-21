using UnityEngine;

// Require a Rigidbody so the GameObject can't miss one in the Inspector
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f; // base movement speed (m/s)
    [SerializeField] private float sprintMultiplier = 1.5f; // multiplier when sprinting
    [SerializeField] private float acceleration = 10f; // how quickly we reach target speed

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 2f; // desired jump height in meters        
    [SerializeField] private Transform groundCheck; // position to check for ground            
    [SerializeField] private float groundCheckRadius = 0.2f; // radius for the ground check sphere
    [SerializeField] private LayerMask groundMask; // which layers count as ground

    [Header("Camera & Rotation")]
    [SerializeField] private Transform cameraTransform;      // reference to the camera
    [SerializeField] private float rotationSpeed = 10f;      // how fast the player turns to movement dir

    // --- Private runtime fields ---
    private Rigidbody rb;            // cached rigidbody
    private Vector3 inputVector;     // desired movement direction in world space
    private bool isGrounded;         // true when on ground
    private bool jumpRequested;      // set true when player pressed jump (processed in FixedUpdate)

    // --- Unity callbacks---

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal"); // A/D
        float z = Input.GetAxisRaw("Vertical");   // W/S

        // --- CAMERA-RELATIVE MOVEMENT ---
        if (cameraTransform != null)
        {
            // Project camera forward onto XZ plane (ignore vertical tilt)
            Vector3 camForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 camRight = cameraTransform.right;

            // Combine inputs relative to camera
            inputVector = (camRight * x + camForward * z).normalized;
        }
        else
        {
            inputVector = (transform.right * x + transform.forward * z).normalized;
        }

        if (Input.GetButtonDown("Jump"))
            jumpRequested = true;
    }

    private void FixedUpdate()
    {
        // 1) Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        // 2) Calculate target speed (account for sprinting)
        bool sprinting = Input.GetKey(KeyCode.LeftShift) && inputVector.sqrMagnitude > 0.01f;
        float targetSpeed = moveSpeed * (sprinting ? sprintMultiplier : 1f) * inputVector.magnitude;

        // 3) Smooth velocity toward target
        Vector3 currentHorizontalVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        Vector3 desiredHorizontalVel = inputVector * targetSpeed;
        Vector3 newHorizontalVel = Vector3.Lerp(currentHorizontalVel, desiredHorizontalVel, acceleration * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector3(newHorizontalVel.x, rb.linearVelocity.y, newHorizontalVel.z);

        // 4) Handle jumping
        if (jumpRequested)
        {
            if (isGrounded)
            {
                float jumpVelocity = Mathf.Sqrt(2f * jumpHeight * -Physics.gravity.y);
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
            }
            jumpRequested = false;
        }

        // 5) Smoothly rotate player toward movement direction
        Vector3 horizontalMove = new Vector3(inputVector.x, 0f, inputVector.z);
        if (horizontalMove.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(horizontalMove);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

    

