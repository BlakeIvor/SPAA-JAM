using UnityEngine;

public class Customer : MonoBehaviour, IInteractable
{
    [SerializeField] CustomerSO customerData;

    public void Interact()
    {
        DialogueManager.Instance.StartTyping(customerData.orderToString());
    }
}
