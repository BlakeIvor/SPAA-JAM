using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] CustomerSpawner customerSpawner;

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
        customerSpawner.StartGame();
    }
}
