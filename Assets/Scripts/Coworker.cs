using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Coworker : MonoBehaviour
{
    public bool isWorking = false;

    [SerializeField] private Transform workLocation;
    [SerializeField] private Transform breakLocation;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float completionTime = 5f;
    [SerializeField] private float cooldownTime = 5f; // Cooldown time in seconds


    public void SetWorking(bool working)
    {
        isWorking = working;
    }

    private void Update()
    {
        if (isWorking)
        {
            // Go to work location
            if (Vector3.Distance(transform.position, workLocation.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, workLocation.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                // Start working
                cooldownTime -= Time.deltaTime;
                if (cooldownTime <= 0f)
                {
                    TryCompleteOrder();
                }
            }
        }
        else
        {
            // Go to break location
            if (Vector3.Distance(transform.position, breakLocation.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, breakLocation.position, moveSpeed * Time.deltaTime);
            }
        }
    }

    private void TryCompleteOrder()
    {
        FoodSpawner.FlavorType[] flavorArr = (FoodSpawner.FlavorType[])Enum.GetValues(typeof(FoodSpawner.FlavorType));

        Customer customer = CustomerSpawner.Instance.getCustomerInQueue(0);
        if (customer != null && customer.State == CustomerState.WaitingForOrder)
        {
            // Complete the customer's order
            customer.Interact(flavorArr);
            cooldownTime = completionTime; // Reset cooldown time for the next order
        }
        else
        {
            if (customer != null && customer.State == CustomerState.FirstInLine)
            {
                customer.Interact();
                cooldownTime = completionTime; // Reset cooldown time for the next order
            }
        }
    }
}
