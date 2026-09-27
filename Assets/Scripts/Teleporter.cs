using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform targetPosition;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.TryGetComponent<PlayerController>(out _))
        {
            // Teleport the player to the target position
            CharacterController characterController = other.GetComponent<CharacterController>();
            characterController.enabled = false; // Disable the CharacterController to avoid collision issues
            other.transform.position = targetPosition.position;
            characterController.enabled = true; // Re-enable the CharacterController after teleportation
            Debug.Log($"Teleported {other.gameObject.name} to {targetPosition.position}");

            // Optionally, add transition animation or effect here
        }
    }

    private void Update()
    {
        
    }
}
