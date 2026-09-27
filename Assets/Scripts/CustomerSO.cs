using UnityEngine;

[CreateAssetMenu(fileName = "Customer", menuName = "Scriptable Objects/Customer")]
public class CustomerSO : ScriptableObject
{
    public Material customerMaterial;
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
            orderString += $"\n- {order.quantity} of something {order.flavor}";
        }

        return orderString;
    }
    
}
