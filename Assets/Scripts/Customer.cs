using UnityEngine;

public class Customer : MonoBehaviour, IInteractable
{
    [SerializeField] CustomerSO customerData;

    public void Initialize(CustomerSO data)
    {
        customerData = data;
    }

    public void Interact()
    {
        DialogueManager.Instance.StartTyping(customerData.orderToString());
    }
}
