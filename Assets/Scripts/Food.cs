using Unity.VisualScripting;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private FoodSpawner.FlavorType[] flavor;
    [SerializeField] private float castRadius = 3f;
    public void OnDrop()
    {
        // Cast a sphere to check for nearby customers, if dropped near a customer, trigger their reaction
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, castRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out Customer customer))
            {
                if (customer.Interact(flavor))
                {
                    Destroy(gameObject);
                    Interactor.Instance.playerStamina.SubtractStamina(10f);
                }
            }
        }
    }
}
