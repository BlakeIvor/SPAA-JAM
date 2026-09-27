using UnityEngine;

public class OrderTicketManager : MonoBehaviour
{
    public static OrderTicketManager Instance { get; private set; }

    [SerializeField] private GameObject orderTicketPrefab;
    [SerializeField] private GameObject ticketContainer;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void CreateOrderTicket(CustomerSO customerData)
    {
        GameObject newTicket = Instantiate(orderTicketPrefab, ticketContainer.transform);
        OrderTicketUI ticketUI = newTicket.GetComponent<OrderTicketUI>();
        if (ticketUI != null)
        {
            ticketUI.SetOrderDetails(customerData);
        }
    }
}
