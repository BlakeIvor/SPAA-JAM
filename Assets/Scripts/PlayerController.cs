using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 verticalVelocity;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Read left/right input using the new Input System (keyboard + gamepad)
        float horizontal = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontal -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontal += 1f;
        }

        if (Gamepad.current != null)
        {
            // Gamepad left stick x axis
            horizontal += Gamepad.current.leftStick.x.ReadValue();
        }

        horizontal = Mathf.Clamp(horizontal, -1f, 1f);

        // Move only left/right relative to the player's transform
        Vector3 move = transform.right * horizontal * moveSpeed;

        // Simple gravity handling to keep the CharacterController grounded
        if (controller.isGrounded && verticalVelocity.y < 0f)
        {
            verticalVelocity.y = -1f;
        }

        verticalVelocity.y += gravity * Time.deltaTime;

        // Apply movement (horizontal + vertical)
        controller.Move((move + verticalVelocity) * Time.deltaTime);
    }
}
