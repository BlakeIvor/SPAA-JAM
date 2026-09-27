using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] PauseMenu pauseMenu;
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
        
    }
    

    public void PauseGame()
    {
        Time.timeScale = 0f; 
        isPaused = true;
        pauseMenu.gameObject.SetActive(true); 
    }
}
