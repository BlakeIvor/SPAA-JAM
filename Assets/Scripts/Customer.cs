using System;
using UnityEngine;

public enum CustomerState
{
    InLine,
    FirstInLine,
    WaitingForOrder,
    CompletedOrder
}

public class Customer : MonoBehaviour, IInteractable
{
    public string interactMessage { get; set; } = "Talk to Customer";
    [SerializeField] CustomerSO customerData;
    [SerializeField] float moveSpeed = 2f;

    public CustomerState State { get; private set; } = CustomerState.InLine;

    private Transform[] walkInWaypoints;
    private Transform[] walkOutWaypoints;
    private Action<Customer> onExit;
    private Customer customerAhead;
    private float queueSpacing;
    private int waypointIndex;

    public void Initialize(CustomerSO data, Transform[] walkInPath, Transform[] walkOutPath, float spacing, Action<Customer> exitCallback)
    {
        customerData = data;
        walkInWaypoints = walkInPath;
        walkOutWaypoints = walkOutPath;
        queueSpacing = Mathf.Max(0f, spacing);
        onExit = exitCallback;
        State = CustomerState.InLine;
        waypointIndex = 1;
    }

    private void Update()
    {
        if (State == CustomerState.InLine)
        {
            FollowPath(walkInWaypoints, FirstInLine);
        }
        else if (State == CustomerState.CompletedOrder)
        {
            FollowPath(walkOutWaypoints, Leave);
        }
    }

    private void FollowPath(Transform[] path, Action reachedEnd)
    {
        if (path == null || path.Length == 0)
        {
            reachedEnd();
            return;
        }

        if (waypointIndex >= path.Length)
        {
            reachedEnd();
            return;
        }

        Transform target = path[waypointIndex];
        Vector3 nextPosition = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if (State == CustomerState.InLine && customerAhead != null)
        {
            float distanceToCustomerAhead = Vector3.Distance(nextPosition, customerAhead.transform.position);

            if (distanceToCustomerAhead < queueSpacing)
            {
                Vector3 awayFromCustomerAhead = nextPosition - customerAhead.transform.position;
                nextPosition = awayFromCustomerAhead.sqrMagnitude > 0f
                    ? customerAhead.transform.position + awayFromCustomerAhead.normalized * queueSpacing
                    : transform.position;
            }
        }

        transform.position = nextPosition;

        if (transform.position == target.position)
        {
            waypointIndex++;
        }
    }

    public void SetCustomerAhead(Customer customer)
    {
        customerAhead = customer;
    }

    private void FirstInLine()
    {
        State = CustomerState.FirstInLine;
        waypointIndex = 0;
    }

    private void WaitingForOrder()
    {
        State = CustomerState.WaitingForOrder;
        waypointIndex = 0;
    }

    public void CompleteOrder()
    {
        if (State != CustomerState.WaitingForOrder)
        {
            return;
        }

        State = CustomerState.CompletedOrder;
        waypointIndex = 0;

        // Remove the order ticket from the OrderTicketManager
        OrderTicketManager.Instance.RemoveOrderTicket(customerData);
    }

    private void Leave()
    {
        onExit?.Invoke(this);
        Destroy(gameObject);
    }

    public void Interact(Interactor interactor)
    {
        if (State == CustomerState.FirstInLine)
        {
            DialogueManager.Instance.StartDialogue(customerData.orderToString());
            OrderTicketManager.Instance.CreateOrderTicket(customerData);
            State = CustomerState.WaitingForOrder;
            Debug.Log("Customer is now waiting for order.");
            interactMessage = "Give order to customer";
        }
        else if (State == CustomerState.WaitingForOrder)
        {
            // Give order to player logic here
            //CompleteOrder(); Disabled for now, as we want to wait for the player to give the item to the customer
        }
    }

    public void Interact()
    {
        // Method for customer to interact that doesn't use dialogue but allows the customer to transition the state to waiting for order
        if (State == CustomerState.FirstInLine)
        {
            State = CustomerState.WaitingForOrder;
            Debug.Log("Customer is now waiting for order.");
            interactMessage = "Give order to customer";
        }
    }

    public bool Interact(FoodSpawner.FlavorType[] flavors)
    {
        if(State == CustomerState.WaitingForOrder)
        {
            bool goodRecommendation = HasMatchingFlavor(flavors);
            CompleteOrder();
            Debug.Log("Customer received order and is satisfied!");
            GameManager.Instance.RecordCustomerServed(goodRecommendation);
            return true;
        }
        return false;
    }

    private bool HasMatchingFlavor(FoodSpawner.FlavorType[] flavors)
    {
        if (customerData == null || customerData.customerOrder == null || flavors == null)
        {
            return false;
        }

        foreach (Order order in customerData.customerOrder)
        {
            if (order == null)
            {
                continue;
            }

            foreach (FoodSpawner.FlavorType flavor in flavors)
            {
                if (order.flavor == flavor)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
