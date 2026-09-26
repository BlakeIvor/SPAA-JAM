using UnityEngine;

public interface IInteractable
{
    public string interactMessage { get; set; }
    public abstract void Interact(Interactor interactor);
}
