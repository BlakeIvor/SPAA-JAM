using System.Collections;
using TMPro;
using UnityEngine;

public class BreakroomEntry : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform breakroom; // The position to teleport the player to

    public string interactMessage { get; set; } = "Switch off with your coworker?";

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

        gameObject.SetActive(false); // Optionally deactivate the entry point after use
    }
}
