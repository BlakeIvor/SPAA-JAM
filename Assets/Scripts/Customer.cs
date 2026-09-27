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

        MeshRenderer meshRenderer = this.transform.GetChild(0)?.GetComponent<MeshRenderer>();
        Debug.Log(meshRenderer.gameObject.name);
        if (meshRenderer != null && customerData != null)
        {
            meshRenderer.material = customerData.customerMaterial;
        }

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

        Transform currentWaypoint = path[Mathf.Max(0, waypointIndex - 1)];
        Transform target = path[waypointIndex];
        bool reachedTargetAxis;
        Vector3 nextPosition = MoveAlongWaypointAxis(
            currentWaypoint.position,
            target.position,
            out reachedTargetAxis);

        if (State == CustomerState.InLine && customerAhead != null)
        {
            float distanceToCustomerAhead = Vector3.Distance(nextPosition, customerAhead.transform.position);

            if (distanceToCustomerAhead < queueSpacing)
            {
                nextPosition = transform.position;
                reachedTargetAxis = false;
            }
        }

        transform.position = nextPosition;

        if (reachedTargetAxis)
        {
            transform.position = target.position;
            waypointIndex++;
        }
    }

    private Vector3 MoveAlongWaypointAxis(
        Vector3 currentWaypoint,
        Vector3 targetWaypoint,
        out bool reachedTargetAxis)
    {
        Vector3 nextPosition = transform.position;
        float deltaX = Mathf.Abs(targetWaypoint.x - currentWaypoint.x);
        float deltaZ = Mathf.Abs(targetWaypoint.z - currentWaypoint.z);
        reachedTargetAxis = false;

        if (deltaX >= deltaZ)
        {
            nextPosition.z = currentWaypoint.z;
            nextPosition.x = Mathf.MoveTowards(
                transform.position.x,
                targetWaypoint.x,
                moveSpeed * Time.deltaTime);
            reachedTargetAxis = Mathf.Approximately(nextPosition.x, targetWaypoint.x);
        }
        else
        {
            nextPosition.x = currentWaypoint.x;
            nextPosition.z = Mathf.MoveTowards(
                transform.position.z,
                targetWaypoint.z,
                moveSpeed * Time.deltaTime);
            reachedTargetAxis = Mathf.Approximately(nextPosition.z, targetWaypoint.z);
        }

        nextPosition.y = transform.position.y;
        return nextPosition;
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
            interactMessage = "";
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
            interactMessage = "";
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
