using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public Transform HoldPoint;
    [SerializeField] TextMeshProUGUI InteractText;
    public Vector3 ThrowDirection => transform.forward;

    // Specifically tracks if we are currently holding a grabbable object
    private IGrabbable currentHeldObject;

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Case 1: We are currently holding an object, so press 'E' to drop it
            if (currentHeldObject != null)
            {
                currentHeldObject.Drop(this);
                currentHeldObject = null;
                return;
            }

            // Case 2: Hand is empty, look for an interactable object
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 3f))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    // If it is specifically a grabbable object, store a reference to it
                    if (interactable is IGrabbable grabbable)
                    {
                        currentHeldObject = grabbable;
                    }

                    // Trigger the interaction (works for both standard items and grabbed items)
                    interactable.Interact(this);
                }
            }
        }
        else
        {
            if (currentHeldObject != null)
            {
                InteractText.text = "";
                return;
            }

            // Raycast to check if any interactable object is in front of the player, and if so, set text to its interact prompt
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 3f))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    InteractText.text = interactable.interactMessage;
                }
            }
            else
            {
                InteractText.text = "";
            }
        }
    }
}