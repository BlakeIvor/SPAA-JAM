using System;
using UnityEngine;

public class FoodSpawner : MonoBehaviour, IInteractable
{
    public string interactMessage => "Press E to respawn food";

    private GameObject[] spawnedObjects;
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
        // Implement the interaction logic here
        Debug.Log("FoodSpawner interacted with by " + interactor.name);

        for (int i = 0; i < spawnedObjects.Length; i++)
        {
            if (spawnedObjects[i] != null)
            {
                // Example interaction: Destroy the spawned food object
                Destroy(spawnedObjects[i]);
                spawnedObjects[i] = null;
                Debug.Log("Destroyed spawned food object at index " + i);
            }
        }

        // Respawn food objects in spawn locations
        for (int i = 0; i < spawnLocations.Length; i++)
        {
            if (spawnLocations[i].foodPrefab != null)
            {
                // Example: Instantiate a new food object at the spawn location
                GameObject newFood = Instantiate(spawnLocations[i].foodPrefab, spawnLocations[i].position.position, spawnLocations[i].position.rotation);
                spawnedObjects[i] = newFood;
                Debug.Log("Respawned food object at index " + i);
            }
        }
    }

    [Serializable]
    public struct FoodPosition
    {
        public Transform position;
        public GameObject foodPrefab;
    }
}
