using UnityEngine;

public class GrabbableObject : MonoBehaviour, IGrabbable
{
    [SerializeField] private string objectInteractMessage = "Grab Object";
    private Rigidbody rb;

    public string InteractMessage => objectInteractMessage;

    private void Awake() => rb = GetComponent<Rigidbody>();

    public void Interact(Interactor interactor)
    {
        rb.isKinematic = true;
        transform.SetParent(interactor.HoldPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop(Interactor interactor)
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        // Apply a force to the object when it is dropped
        //rb.AddForce(interactor.ThrowDirection * 5f, ForceMode.Impulse);
    }
}