using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float currentStamina = 60f;

    [Header("UI")]
    [SerializeField] private Slider staminaSlider;

    private void Awake()
    {
        // Make sure stamina starts within valid range
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        // Configure the slider as a display-only fill bar
        if (staminaSlider != null)
        {
            staminaSlider.minValue = 0f;
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
            staminaSlider.interactable = false;
        }

        UpdateSlider();
    }

    /// <summary>
    /// Removes stamina.
    /// </summary>
    public void SubtractStamina(float amount)
    {
        if (amount <= 0f)
            return;

        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        UpdateSlider();
    }

    /// <summary>
    /// Adds stamina.
    /// </summary>
    public void AddStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        UpdateSlider();
    }

    public void ApplyTemporaryBoost(float amount, float duration)
    {
        if (amount <= 0f || duration <= 0f)
        {
            return;
        }

        StartCoroutine(TemporaryBoost(amount, duration));
    }

    private IEnumerator TemporaryBoost(float amount, float duration)
    {
        float boostAmount = Mathf.Min(amount, maxStamina - currentStamina);
        AddStamina(boostAmount);

        float remainingBoost = boostAmount;
        while (remainingBoost > 0f)
        {
            yield return null;

            float staminaToRemove = Mathf.Min(
                remainingBoost,
                boostAmount * Time.deltaTime / duration);
            SubtractStamina(staminaToRemove);
            remainingBoost -= staminaToRemove;
        }
    }

    /// <summary>
    /// Returns the current stamina value.
    /// </summary>
    public float GetStamina()
    {
        return currentStamina;
    }

    /// <summary>
    /// Returns the maximum stamina value.
    /// </summary>
    public float GetMaxStamina()
    {
        return maxStamina;
    }

    /// <summary>
    /// Returns stamina as a value from 0 to 1.
    /// </summary>
    public float GetStaminaPercentage()
    {
        return currentStamina / maxStamina;
    }

    private void UpdateSlider()
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
        }
    }


}
