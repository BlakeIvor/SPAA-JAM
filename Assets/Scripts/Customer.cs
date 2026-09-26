using UnityEngine;

public class Customer : MonoBehaviour, IInteractable
{
    [SerializeField] CustomerSO customerData;

    public void Interact(Interactor interactor)
    {
        DialogueManager.Instance.StartTyping(customerData.orderToString());
    }
}
