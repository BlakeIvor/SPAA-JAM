using UnityEngine;

public class BedInteractable : MonoBehaviour, IInteractable
{
    public string interactMessage { get; set; } = "Go to next day";

    public void Interact(Interactor interactor)
    {
        GameManager.Instance.GoToNextDay();
    }
}
