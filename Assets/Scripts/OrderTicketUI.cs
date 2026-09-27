using UnityEngine;
using TMPro;

public class OrderTicketUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI orderDetailsText;

    public void SetOrderDetails(CustomerSO customerData)
    {
        string orderString = "";
        foreach (Order order in customerData.customerOrder)
        {
            orderString += $"\n- {order.quantity} {order.flavor} item(s)";
        }
        orderDetailsText.text = orderString;
    }

    
}
