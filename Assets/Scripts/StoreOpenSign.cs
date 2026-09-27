using UnityEngine;

public class StoreOpenSign : MonoBehaviour, IInteractable
{
    public string interactMessage { get; set; }

    void Start()
    {
        GameManager.Instance.OnStoreOpenChanged += UpdateSign;
        UpdateSign();
    }

    private void UpdateSign()
    {
        interactMessage = GameManager.Instance.storeOpen ? "Close Store" : "Open Store";
    }

    public void Interact(Interactor interactor)
    {
        GameManager.Instance.ToggleStoreOpen();
        UpdateSign();
    }
}
