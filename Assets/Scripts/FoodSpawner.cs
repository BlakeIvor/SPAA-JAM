using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FoodSpawner : MonoBehaviour, IInteractable
{
    public string interactMessage { get; set; } = "Respawn food";

    [SerializeField]private float cooldownTime = 5f; // Cooldown time in seconds
    [SerializeField] private float remainingTime = 0; // Remaining cooldown time

    private string defaultInteractMessage = "Respawn food";

    private List<GameObject> spawnedObjects = new List<GameObject>();
    [SerializeField] private FoodPosition[] spawnLocations;
    public enum FlavorType
    {
        Sweet,
        Savory,
        Energizing,
        Refreshing,
        Soothing,
        Salty,
    }

    public void Interact(Interactor interactor)
    {
        if (remainingTime > 0)
        {
            return;
        }
        else
        {
            // Implement the interaction logic here
            Debug.Log("FoodSpawner interacted with by " + interactor.name);
            for (int i = 0; i < spawnedObjects.Count; i++)
            {
                if (spawnedObjects[i] != null)
                {
                    Destroy(spawnedObjects[i]);
                }
            }

            spawnedObjects.Clear();

            // Respawn food objects in spawn locations
            for (int i = 0; i < spawnLocations.Length; i++)
            {
                if (spawnLocations[i].foodPrefab != null)
                {
                    // Example: Instantiate a new food object at the spawn location
                    GameObject newFood = Instantiate(spawnLocations[i].foodPrefab, spawnLocations[i].position.position, spawnLocations[i].position.rotation);
                    spawnedObjects.Add(newFood);
                    Debug.Log("Respawned food object at index " + i);
                }
            }
            StartCoroutine(Cooldown());
        }
    }

    [Serializable]
    public struct FoodPosition
    {
        public Transform position;
        public GameObject foodPrefab;
    }

    private IEnumerator Cooldown()
    {
        remainingTime = cooldownTime;
        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
            interactMessage = $"Respawn food (Cooldown: {remainingTime}s)";
        }

        interactMessage = defaultInteractMessage; // Reset the interact message after cooldown
    }
}
