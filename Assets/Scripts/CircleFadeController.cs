using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CircleFadeController : MonoBehaviour
{
    [Header("Transition Settings")]
    [SerializeField] private Material fadeMaterial;
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float holdDuration = 1.5f; // Time screen stays dark

    private static readonly int RadiusID = Shader.PropertyToID("_Radius");
    private bool isTransitioning = false;

    void Start()
    {
        if (GetComponent<Image>() != null)
        {
            fadeMaterial = GetComponent<Image>().material;
        }
        
        // Start open (visible screen)
        fadeMaterial.SetFloat(RadiusID, 1.5f); 
    }

    /// <summary>
    /// Call this function to run the full sequence: Fade Out -> Hold -> Fade In.
    /// </summary>
    [ContextMenu("Trigger Full Transition")]
    public void TriggerFullTransition()
    {
        if (isTransitioning) return; // Prevent overlapping inputs
        StartCoroutine(FullTransitionRoutine());
    }

    private IEnumerator FullTransitionRoutine()
    {
        isTransitioning = true;

        // 1. Fade Out (Shrink circle to 0)
        yield return StartCoroutine(FadeRoutine(1.5f, 0.0f));

        // 2. Wait / Hold screen black
        // (Great spot to load a new scene or teleport your player)
        yield return new WaitForSeconds(holdDuration);

        // 3. Fade In (Expand circle back to 1.5)
        yield return StartCoroutine(FadeRoutine(0.0f, 1.5f));

        isTransitioning = false;
    }

    private IEnumerator FadeRoutine(float startRadius, float targetRadius)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentRadius = Mathf.Lerp(startRadius, targetRadius, elapsedTime / fadeDuration);
            fadeMaterial.SetFloat(RadiusID, currentRadius);
            yield return null;
        }

        fadeMaterial.SetFloat(RadiusID, targetRadius);
    }
}
