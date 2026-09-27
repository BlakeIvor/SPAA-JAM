using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner Instance { get; private set; }
    [SerializeField] float spawnInterval = 12f;
    [SerializeField] float queueSpacing = 1.25f;
    [SerializeField] GameObject customerPrefab;
    [SerializeField] CustomerSO[] customerTypes;
    [SerializeField] Transform[] walkInWaypoints;
    [SerializeField] Transform[] walkOutWaypoints;

    private readonly List<Customer> customersInQueue = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartGame()
    {
        StartCoroutine(SpawnCustomer());
    }

    private IEnumerator SpawnCustomer()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (GameManager.Instance.storeOpen)
            {
                SpawnCustomerInstance();
            }
        }
    }

    private void SpawnCustomerInstance()
    {
        if (customerPrefab == null || customerTypes == null || customerTypes.Length == 0 || walkInWaypoints == null || walkInWaypoints.Length == 0)
        {
            Debug.LogWarning("CustomerSpawner is missing a prefab, customer type, or walk-in waypoint.", this);
            return;
        }

        GameObject customerInstance = Instantiate(customerPrefab, walkInWaypoints[0].position, Quaternion.identity);
        Customer customer = customerInstance.GetComponent<Customer>();

        if (customer == null)
        {
            Debug.LogError("The customer prefab must have a Customer component.", customerInstance);
            Destroy(customerInstance);
            return;
        }

        Customer customerAhead = customersInQueue.Count > 0
            ? customersInQueue[customersInQueue.Count - 1]
            : null;

        customer.Initialize(customerTypes[Random.Range(0, customerTypes.Length)], walkInWaypoints, walkOutWaypoints, queueSpacing, CustomerLeft);
        customer.SetCustomerAhead(customerAhead);
        customersInQueue.Add(customer);
    }

    private void CustomerLeft(Customer customer)
    {
        customersInQueue.Remove(customer);
        UpdateQueueLinks();
    }

    private void UpdateQueueLinks()
    {
        for (int customerIndex = 0; customerIndex < customersInQueue.Count; customerIndex++)
        {
            Customer customerAhead = customerIndex > 0 ? customersInQueue[customerIndex - 1] : null;
            customersInQueue[customerIndex].SetCustomerAhead(customerAhead);
        }
    }

    public int getNumCustomersInQueue()
    {
        return customersInQueue.Count;
    }

    public Customer getCustomerInQueue(int index)
    {
        if (index < 0 || index >= customersInQueue.Count)
        {
            return null;
        }
        return customersInQueue[index];
    }
}
