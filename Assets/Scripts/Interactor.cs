using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    // Static reference to the Interactor instance, allowing other scripts to easily access it
    public static Interactor Instance { get; private set; }

    public Stamina playerStamina;

    public Transform HoldPoint;
    [SerializeField] TextMeshProUGUI InteractText;
    public Vector3 ThrowDirection => transform.forward;

    [SerializeField] private float castDistance = 8f;

    // Specifically tracks if we are currently holding a grabbable object
    private IGrabbable currentHeldObject;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueOpen)
        {
            InteractText.text = "";
            return;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Case 1: Game is paused, do not allow interaction, some UI is open probably
            if (GameManager.Instance != null && GameManager.Instance.isPaused)
            {
                return;
            }
            
            // Case 2: We are currently holding an object, so press 'E' to drop it
            if (currentHeldObject != null)
            {
                currentHeldObject.Drop(this);
                currentHeldObject = null;
                return;
            }

            // Case 3: Hand is empty, look for an interactable object
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, castDistance))
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
        else if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (currentHeldObject is Component heldComponent && heldComponent.TryGetComponent(out Food food))
            {
                food.Eat();
                currentHeldObject = null;
                return;
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