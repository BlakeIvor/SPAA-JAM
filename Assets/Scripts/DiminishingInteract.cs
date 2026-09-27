using System;
using Unity.VisualScripting;
using UnityEngine;

public class DiminishingInteract : MonoBehaviour, IInteractable
{
    public string interactMessage { get; set; } = "Interact";
    [SerializeField] private string interactMessageOverride;
    [SerializeField] private float diminishingAmount = 0.5f; // The amount by which the effectiveness diminishes each time
    [SerializeField] private float initialStaminaGain;
    [SerializeField] private float currentStaminaGain;
    [SerializeField] private float minStaminaGain = -3f; // Overdoing it can even lose you stamina!
    [SerializeField] private float timeBetweenUse = 1f; // Time in seconds before the player can use this interactable again
    private float cooldownTimer = 0f; // Timer to track the cooldown

    public void Awake()
    {
        if (!string.IsNullOrEmpty(interactMessageOverride))
        {
            interactMessage = interactMessageOverride + " (Watch out! If you do too much, its not as effective! You might even lose stamina!)";
        }
        currentStaminaGain = initialStaminaGain;
    }

    private void Update()
    {
        if(cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime; // Decrease the cooldown timer over time
        }

        if (cooldownTimer <= 0)
        {
            interactMessage = interactMessageOverride + " (Watch out! If you do too much, its not as effective! You might even feel worse!)";
        }
        else
        {
            interactMessage = "On cooldown! Wait for " + Mathf.Ceil(cooldownTimer) + " seconds.";
        }
    }
    public void Interact(Interactor interactor)
    {
        if(cooldownTimer <= 0)
        {
            interactor.playerStamina.AddStamina(currentStaminaGain);
            Diminish();
            cooldownTimer = timeBetweenUse; // Reset the cooldown timer
        }
    }

    private void Diminish()
    {
        //Diminish the effectiveness of the interaction by the diminishing amount, but not below the minimum stamina gain
        currentStaminaGain = Mathf.Max(currentStaminaGain - diminishingAmount, minStaminaGain);
    }

    public void Reset()
    {
        currentStaminaGain = initialStaminaGain;
    }
}
