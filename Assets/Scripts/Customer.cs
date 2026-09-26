using UnityEngine;

public class Customer : MonoBehaviour, IInteractable
{
    public string interactMessage { get; set; } = "Talk to Customer";
    [SerializeField] CustomerSO customerData;

    public void Initialize(CustomerSO data)
    {
        customerData = data;
    }

    public void Interact(Interactor interactor)
    {
        DialogueManager.Instance.StartTyping(customerData.orderToString());
    }
}
