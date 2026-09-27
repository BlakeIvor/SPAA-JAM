using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DailyGoalNotebook : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject goalChoicePanel;
    [SerializeField] private Transform goalChoiceContainer;
    [SerializeField] private Button goalChoiceButtonPrefab;
    [SerializeField] private Transform goalSelectedContainer;
    [SerializeField] private GameObject goalSelectedPrefab;

    [SerializeField]
    private List<string> goalChoices = new()
    {
        "Serve 3 customers",
        "Serve 5 customers",
        "Serve 10 customers",
        "Take 1 break throughout the day",
        "Take 2 breaks throughout the day",
        "Eat 1 snack throughout the day",
        "Eat 2 snacks throughout the day",
        "Serve 2 customer w/ Good Recommendation",
        "Serve 3 customer w/ Good Recommendation",
        "Serve 5 customer w/ Good Recommendation"
    };

    [SerializeField] private UnityEvent<string> goalSelected;

    private bool isGoalSelected = false;
    private GameObject selectedGoalObject;
    private TMPro.TMP_Text selectedGoalText;

    private readonly List<Button> choiceButtons = new();

    public string interactMessage { get; set; } = "Choose daily goal";
    public string SelectedGoal { get; private set; }

    private void Start()
    {
        CloseGoalChoices();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnDailyGoalProgressChanged += UpdateSelectedGoalText;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnDailyGoalProgressChanged -= UpdateSelectedGoalText;
        }
    }

    public void Interact(Interactor interactor)
    {
        if (goalChoicePanel != null && goalChoicePanel.activeSelf)
        {
            CloseGoalChoices();
            return;
        }

        if (!isGoalSelected)
        {
            OpenGoalChoices();
        }
    }

    private void OpenGoalChoices()
    {
        if (goalChoicePanel == null ||
            goalChoiceContainer == null ||
            goalChoiceButtonPrefab == null)
        {
            Debug.LogWarning(
                "DailyGoalNotebook needs a panel, container, and button prefab assigned.",
                this
            );

            return;
        }

        if (GameManager.Instance == null)
        {
            Debug.LogWarning(
                "DailyGoalNotebook could not find a GameManager.",
                this
            );

            return;
        }

        GameManager.Instance.isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        ClearGoalChoices();

        goalChoicePanel.SetActive(true);

        List<string> availableGoals = new(goalChoices);
        int choicesToShow = Mathf.Min(3, availableGoals.Count);

        for (int choiceIndex = 0; choiceIndex < choicesToShow; choiceIndex++)
        {
            int randomIndex = Random.Range(0, availableGoals.Count);

            string goalChoice = availableGoals[randomIndex];
            availableGoals.RemoveAt(randomIndex);

            Button choiceButton =
                Instantiate(goalChoiceButtonPrefab, goalChoiceContainer);

            TMPro.TMP_Text choiceText =
                choiceButton.GetComponentInChildren<TMPro.TMP_Text>(true);

            if (choiceText == null)
            {
                Debug.LogWarning(
                    "DailyGoalNotebook button prefab needs a TMP text component.",
                    choiceButton
                );

                Destroy(choiceButton.gameObject);
                continue;
            }

            choiceText.text = goalChoice;

            string capturedGoalChoice = goalChoice;

            choiceButton.onClick.AddListener(
                () => SelectGoal(capturedGoalChoice)
            );

            choiceButtons.Add(choiceButton);
        }
    }

    private void SelectGoal(string goalChoice)
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning(
                "DailyGoalNotebook could not find a GameManager.",
                this
            );

            return;
        }

        SelectedGoal = goalChoice;

        GameManager.Instance.SetDailyGoal(goalChoice);

        DestroySelectedGoalNote();

        if (goalSelectedPrefab != null && goalSelectedContainer != null)
        {
            selectedGoalObject =
                Instantiate(goalSelectedPrefab, goalSelectedContainer);

            selectedGoalText =
                selectedGoalObject.GetComponentInChildren<TMPro.TMP_Text>(true);
        }
        else
        {
            Debug.LogWarning(
                "DailyGoalNotebook needs a selected goal prefab and container assigned.",
                this
            );
        }

        UpdateSelectedGoalText();

        goalSelected?.Invoke(goalChoice);

        interactMessage = "Daily goal selected: " + goalChoice;

        isGoalSelected = true;

        CloseGoalChoices();
    }

    private void CloseGoalChoices()
    {
        if (goalChoicePanel != null)
        {
            goalChoicePanel.SetActive(false);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.isPaused = false;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ClearGoalChoices()
    {
        foreach (Button choiceButton in choiceButtons)
        {
            if (choiceButton != null)
            {
                Destroy(choiceButton.gameObject);
            }
        }

        choiceButtons.Clear();
    }

    private void UpdateSelectedGoalText()
    {
        if (string.IsNullOrEmpty(SelectedGoal) ||
            selectedGoalText == null ||
            GameManager.Instance == null)
        {
            return;
        }

        string[] goalParts = SelectedGoal.Split(' ');

        if (goalParts.Length > 1 &&
            int.TryParse(goalParts[1], out int requiredAmount))
        {
            int currentProgress =
                GameManager.Instance.GetDailyGoalProgress();

            selectedGoalText.text =
                $"{SelectedGoal} ({currentProgress}/{requiredAmount})";
        }
        else
        {
            selectedGoalText.text = SelectedGoal;
        }
    }

    private void DestroySelectedGoalNote()
    {
        if (selectedGoalObject != null)
        {
            Destroy(selectedGoalObject);
        }

        selectedGoalObject = null;
        selectedGoalText = null;
    }

    public void ResetDailyGoal()
    {
        SelectedGoal = null;
        isGoalSelected = false;

        DestroySelectedGoalNote();

        interactMessage = "Choose daily goal";
    }
}