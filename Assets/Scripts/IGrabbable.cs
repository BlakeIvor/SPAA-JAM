using UnityEngine;

public interface IGrabbable : IInteractable
{
    public abstract void Interact(Interactor interactor);
    public abstract void Drop(Interactor interactor);
}
