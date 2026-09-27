using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI.Table;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;

        bool dialogueOpen = DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueOpen;

        // Keyboard input
        if (!dialogueOpen && Keyboard.current != null)
        {
            // Left / Right
            if (Keyboard.current.aKey.isPressed ||
                Keyboard.current.leftArrowKey.isPressed)
            {
                horizontal -= 1f;
            }

            if (Keyboard.current.dKey.isPressed ||
                Keyboard.current.rightArrowKey.isPressed)
            {
                horizontal += 1f;
            }

            // Forward / Backward
            if (Keyboard.current.wKey.isPressed ||
                Keyboard.current.upArrowKey.isPressed)
            {
                vertical += 1f;
            }

            if (Keyboard.current.sKey.isPressed ||
                Keyboard.current.downArrowKey.isPressed)
            {
                vertical -= 1f;
            }
        }

        // Gamepad input
        if (!dialogueOpen && Gamepad.current != null)
        {
            horizontal += Gamepad.current.leftStick.x.ReadValue();
            vertical += Gamepad.current.leftStick.y.ReadValue();
        }

        // Clamp input
        horizontal = Mathf.Clamp(horizontal, -1f, 1f);
        vertical = Mathf.Clamp(vertical, -1f, 1f);

        // Movement relative to player's rotation
        Vector3 move =
            transform.right * horizontal +
            transform.forward * vertical;

        move *= moveSpeed;

        // Gravity
        if (controller.isGrounded && verticalVelocity.y < 0f)
        {
            verticalVelocity.y = -1f;
        }

        verticalVelocity.y += gravity * Time.deltaTime;

        // Apply movement
        controller.Move(
            (move + verticalVelocity) * Time.deltaTime
        );
    }
}
