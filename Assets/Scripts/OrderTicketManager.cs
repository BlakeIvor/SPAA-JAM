using System;
using UnityEngine;

public class OrderTicketManager : MonoBehaviour
{
    public static OrderTicketManager Instance { get; private set; }

    [SerializeField] private GameObject orderTicketPrefab;
    [SerializeField] private GameObject ticketContainer;

    private readonly System.Collections.Generic.List<OrderTicketUI> ticketList = new System.Collections.Generic.List<OrderTicketUI>();

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
        ticketList.Add(ticketUI);
    }

    public void RemoveOrderTicket(CustomerSO customerData)
    {
        try
        {
            var ticket = ticketList[0];
            ticketList.RemoveAt(0);
            Destroy(ticket.gameObject);
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.LogWarning("No order tickets to remove.", this);
        }
    }
}
