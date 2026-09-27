using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

public class DailyGoalNotebook : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject goalChoicePanel;
    [SerializeField] private Transform goalChoiceContainer;
    [SerializeField] private Button goalChoiceButtonPrefab;
    [SerializeField] private Transform goalSelectedContainer;
    [SerializeField] private GameObject goalSelectedPrefab;
    [SerializeField] private List<string> goalChoices = new()
    {
        "Serve 5 customers",
        "Complete 3 orders",
        "Restock all food"
    };
    [SerializeField] private UnityEvent<string> goalSelected;
    private bool isGoalSelected = false;

    public string interactMessage { get; set; } = "Choose daily goal";
    public string SelectedGoal { get; private set; }

    private readonly List<Button> choiceButtons = new();

    private void Start()
    {
        CloseGoalChoices();
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
        GameManager.Instance.isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (goalChoicePanel == null || goalChoiceContainer == null || goalChoiceButtonPrefab == null)
        {
            Debug.LogWarning("DailyGoalNotebook needs a panel, container, and button prefab assigned.", this);
            return;
        }

        ClearGoalChoices();
        goalChoicePanel.SetActive(true);

        List<string> availableGoals = new(goalChoices);
        int choicesToShow = Mathf.Min(3, availableGoals.Count);

        for (int choiceIndex = 0; choiceIndex < choicesToShow; choiceIndex++)
        {
            int randomIndex = Random.Range(0, availableGoals.Count);
            string goalChoice = availableGoals[randomIndex];
            availableGoals.RemoveAt(randomIndex);

            Button choiceButton = Instantiate(goalChoiceButtonPrefab, goalChoiceContainer);
            TMPro.TMP_Text choiceText = choiceButton.GetComponentInChildren<TMPro.TMP_Text>();
            if (choiceText == null)
            {
                Debug.LogWarning("DailyGoalNotebook button prefab needs a TMP text component.", choiceButton);
                Destroy(choiceButton.gameObject);
                continue;
            }

            choiceText.text = goalChoice;
            choiceButton.onClick.AddListener(() => SelectGoal(goalChoice));
            choiceButtons.Add(choiceButton);
        }
    }

    private void SelectGoal(string goalChoice)
    {
        SelectedGoal = goalChoice;
        GameObject selectedGoal = Instantiate(goalSelectedPrefab, goalSelectedContainer);
        TMPro.TMP_Text selectedText = selectedGoal.GetComponentInChildren<TMPro.TMP_Text>();
        if (selectedText != null)
        {
            selectedText.text = goalChoice;
        }
        goalSelected?.Invoke(goalChoice);
        interactMessage = "Daily goal selected: " + goalChoice;
        isGoalSelected = true;
        CloseGoalChoices();
    }

    private void CloseGoalChoices()
    {
        if (goalChoicePanel != null)
        {
            GameManager.Instance.isPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            goalChoicePanel.SetActive(false);
        }
    }

    private void ClearGoalChoices()
    {
        foreach (Button choiceButton in choiceButtons)
        {
            Destroy(choiceButton.gameObject);
        }

        choiceButtons.Clear();
    }
}
