using UnityEngine;

public class BedInteractable : MonoBehaviour, IInteractable
{
    CircleFadeController fadeController;
    public string interactMessage { get; set; } = "Go to next day";


    void Start()
    {
        fadeController = GetComponent<CircleFadeController>();
    }
    public void Interact(Interactor interactor)
    {
        fadeController.TriggerFullTransition();
        GameManager.Instance.GoToNextDay();
    }
}
