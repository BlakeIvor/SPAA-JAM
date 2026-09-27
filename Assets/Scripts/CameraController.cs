using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonLook : MonoBehaviour
{
    [Header("References")]
    public Transform playerBody;      // The parent object (horizontal rotation)
    public Transform cameraPivot;     // The child object (vertical rotation)

    [Header("Settings")]
    public float mouseSensitivity = 2.0f;
    
    [Header("Look Limits")]
    public float minVerticalAngle = -85f; // Example: Lock to a span below horizon
    public float maxVerticalAngle = 85f;  // Example: Lock to a span above horizon
    
    // If you want horizontal limits (e.g., only look left/right by 90 degrees)
    public float minHorizontalAngle = -70f;
    public float maxHorizontalAngle = 70f;

    private float yRotation = 0f; // Horizontal tracking
    private float xRotation = 0f; // Vertical tracking

    void Start()
    {
        // Lock cursor for first-person experience
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. Get Input
        float mouseX =  Mouse.current.delta.x.value * mouseSensitivity;
        float mouseY = Mouse.current.delta.y.value * mouseSensitivity;

        // 2. Calculate Rotations
        yRotation += mouseX;
        xRotation -= mouseY; // Invert Y because moving mouse up usually means a smaller rotation angle in Unity scene space

        // 3. Apply Clamping (The "Locked Span" logic)
        // Make sure if set to 360 degrees, it doesn't clamp, otherwise clamp to the specified range
        if (360f - (maxVerticalAngle - minVerticalAngle) < 0.01f)
        {
            // No clamping for vertical rotation
        }
        else
        {
            xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
        }
        if (360f - (maxHorizontalAngle - minHorizontalAngle) < 0.01f)
        {
            // No clamping for horizontal rotation
        }
        else
        {
            yRotation = Mathf.Clamp(yRotation, minHorizontalAngle, maxHorizontalAngle);
        }

        //Apply both rotations to pivot
        //cameraPivot.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // Apply horizontal rotation to the player body (yaw)
        playerBody.localRotation = Quaternion.Euler(0f, yRotation, 0f);

        // Apply vertical rotation to the camera pivot (pitch)
        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
