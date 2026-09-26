using UnityEngine;

public class BillboardComponent : MonoBehaviour
{
    [SerializeField] private bool lockYAxisOnly = true;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera == null) return;

        if (lockYAxisOnly)
        {
            Vector3 lookAtPosition = mainCamera.transform.position;
            lookAtPosition.y = transform.position.y;
            transform.LookAt(lookAtPosition);
        }
        else
        {
            transform.LookAt(mainCamera.transform);
        }
    }
}
