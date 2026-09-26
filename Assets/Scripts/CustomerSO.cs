using UnityEngine;

[CreateAssetMenu(fileName = "Customer", menuName = "Scriptable Objects/Customer")]
public class CustomerSO : ScriptableObject
{
    public Sprite customerSprite;
    public Order[] customerOrder;
    public string orderDialogue;
    public string closingDialogue;
    public int paymentAmount;
    public int maxTip;

    public string orderToString()
    {
        string orderString = orderDialogue;

        foreach (Order order in customerOrder)
        {
            orderString += $"\n- {order.itemName} x{order.quantity}";
        }

        return orderString;
    }
    
}
