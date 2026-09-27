using System.Collections;
using TMPro;
using UnityEngine;

public class TeleportInteract : MonoBehaviour, IInteractable
{
    [SerializeField] bool disableOnInteract = false; // Whether to disable the entry point after interaction
    [SerializeField] private Transform breakroom; // The position to teleport the player to
    [SerializeField] private string interactText; // The text to display the interaction message

    public string interactMessage { get; set; } = "Switch off with your coworker?";

    private void Start()
    {
        // Set the interact message to the serialized field value if it's not empty
        interactMessage = interactText;
    }
    public void Interact(Interactor interactor)
    {
        // Teleport into the breakroom
        StartCoroutine(Enter(interactor));

    }

    private IEnumerator Enter(Interactor interactor)
    {
        yield return new WaitForSeconds(0.5f); // Wait for half a second before teleporting
        // Teleport the player to the breakroom

        CharacterController characterController = interactor.GetComponentInParent<CharacterController>();
        characterController.enabled = false; // Disable the CharacterController to avoid collision issues
        characterController.transform.position = breakroom.position;
        characterController.enabled = true; // Re-enable the CharacterController after teleportation

        if(disableOnInteract) gameObject.SetActive(false); // Optionally deactivate the entry point after use
    }
}
