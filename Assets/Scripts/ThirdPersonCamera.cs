using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target to Follow")]
    public Transform target;   // The player

    [Header("Camera Settings")]
    public float distance = 5f;         // Distance behind player
    public float height = 2f;           // Height above player
    public float rotationSpeed = 3f;    // Mouse sensitivity

    private float yaw = 0f;   // Horizontal rotation
    private float pitch = 0f; // Vertical rotation

    public static ThirdPersonCamera instance; // For easy access

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Camera target not assigned!");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * rotationSpeed;
        pitch -= mouseY * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -30f, 60f);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        Vector3 targetPosition = target.position - (rotation * Vector3.forward * distance) + Vector3.up * height;

        transform.position = targetPosition;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
    
    // 🔹 Helper: gives forward direction ignoring tilt (so player doesn't walk into ground)
    public Vector3 GetForwardDirection()
    {
        Vector3 forward = transform.forward;
        forward.y = 0f;
        return forward.normalized;
    }

    public Vector3 GetRightDirection()
    {
        Vector3 right = transform.right;
        right.y = 0f;
        return right.normalized;
    }
}
