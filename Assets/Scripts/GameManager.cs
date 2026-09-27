using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] Animator sunAnimator;
    [SerializeField] PauseMenu pauseMenu;
    public event Action OnDailyGoalProgressChanged;
    public string SelectedDailyGoal { get; private set; }
    public int CustomersServed { get; private set; }
    public int BreaksTaken { get; private set; }
    public int SnacksEaten { get; private set; }
    public int GoodRecommendations { get; private set; }
    public bool isPaused { get; set; } = false;
    private bool _storeOpen;
    public bool storeOpen
    {
        get => _storeOpen;
        set
        {
            if (_storeOpen == value)
            {
                return;
            }

            _storeOpen = value;
            OnStoreOpenChanged?.Invoke();
        }
    }
    public event Action OnStoreOpenChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Debug.Log("GameManager initialized.");
        Instance = this;
    }

    void Start()
    {
        DialogueManager.Instance.DialogueClosed += StartGame;
        DialogueManager.Instance.StartDialogue("Welcome to [GAME NAME/BAKERY NAME]! Once this dialogue closes, your first day will start.",
            "Your goal is to help customers with their orders and make sure they leave happy.",
            "You can interact with customers and items by pressing 'E' or left clicking on them.",
            "Be sure to take breaks once in a while in the break room to keep your energy up.",
            "Once the day is over, head on upstairs to get a good nights rest.",
            "Good luck and enjoy!");
    }

    private void StartGame()
    {
        DialogueManager.Instance.DialogueClosed -= StartGame;
        CustomerSpawner.Instance.StartGame();
        pauseMenu.gameObject.SetActive(false); 
        storeOpen = true;
    }

    public void ToggleStoreOpen()
    {
        storeOpen = !storeOpen;
    }

    public void GoToNextDay()
    {
        sunAnimator.Play("SunAnimation", 0, 0f);
        Interactor.Instance.playerStamina.AddStamina(100f);
        DailyGoalNotebook notebook = FindFirstObjectByType<DailyGoalNotebook>();
        if (notebook != null)
        {
            notebook.ResetDailyGoal();
        }

        DiminishingInteract[] diminishingInteracts = FindObjectsOfType<DiminishingInteract>();
        foreach (DiminishingInteract interact in diminishingInteracts)
        {
            interact.Reset();
        }
    }

    public void SetDailyGoal(string goal)
    {
        SelectedDailyGoal = goal;
        CustomersServed = 0;
        BreaksTaken = 0;
        SnacksEaten = 0;
        GoodRecommendations = 0;
        OnDailyGoalProgressChanged?.Invoke();
    }

    public void RecordCustomerServed(bool goodRecommendation)
    {
        CustomersServed++;
        if (goodRecommendation)
        {
            GoodRecommendations++;
        }

        OnDailyGoalProgressChanged?.Invoke();
    }

    public void RecordBreak()
    {
        BreaksTaken++;
        OnDailyGoalProgressChanged?.Invoke();
    }

    public void RecordSnack()
    {
        SnacksEaten++;
        OnDailyGoalProgressChanged?.Invoke();
    }

    public int GetDailyGoalProgress()
    {
        if (string.IsNullOrEmpty(SelectedDailyGoal))
        {
            return 0;
        }

        if (SelectedDailyGoal.Contains("Good Recommendation"))
        {
            return GoodRecommendations;
        }

        if (SelectedDailyGoal.Contains("break"))
        {
            return BreaksTaken;
        }

        if (SelectedDailyGoal.Contains("snack"))
        {
            return SnacksEaten;
        }

        return CustomersServed;
    }

    public bool IsDailyGoalComplete()
    {
        if (string.IsNullOrEmpty(SelectedDailyGoal))
        {
            return false;
        }

        string[] goalParts = SelectedDailyGoal.Split(' ');
        if (!int.TryParse(goalParts[1], out int requiredAmount))
        {
            return false;
        }

        return GetDailyGoalProgress() >= requiredAmount;
    }
        
    public void PauseGame()
    {
        Time.timeScale = 0f; 
        isPaused = true;
        pauseMenu.gameObject.SetActive(true); 
    }
}
