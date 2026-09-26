using UnityEngine;

public interface IInteractable
{
    public string interactMessage { get; }
    public abstract void Interact(Interactor interactor);
}
