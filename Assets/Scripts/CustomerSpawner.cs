using System.Collections;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public int currentCustomers = 0;
    public int maxCustomers = 5;
    [SerializeField] float spawnInterval = 12f;
    [SerializeField] GameObject customerPrefab;
    [SerializeField] CustomerSO[] customerTypes;
    [SerializeField] Transform[] walkInWaypoints;
    [SerializeField] Transform[] walkOutWaypoints;

    void Start()
    {
        StartCoroutine(SpawnCustomer());
    }

    private IEnumerator SpawnCustomer()
    {
        while (true && currentCustomers < maxCustomers)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnCustomerInstance();
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

        customer.Initialize(customerTypes[Random.Range(0, customerTypes.Length)]);
        currentCustomers++;
    }
}
