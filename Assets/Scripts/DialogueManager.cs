using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public float timeBetweenChar = 0.04f;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] TextMeshProUGUI textComponent;
    private Coroutine typingCoroutine;
    private string[] dialogueChain;
    private int dialogueIndex;
    private bool isTyping;
    private bool waitForAdvanceRelease;

    public bool IsDialogueOpen { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartDialogue(params string[] chain)
    {
        if (chain == null || chain.Length == 0)
        {
            return;
        }

        dialogueChain = chain;
        dialogueIndex = 0;
        IsDialogueOpen = true;
        waitForAdvanceRelease = true;
        dialogueBox.SetActive(true);
        ShowCurrentDialogue();
    }

    private void Update()
    {
        if (!IsDialogueOpen)
        {
            return;
        }

        bool advancePressed = (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            || (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);

        if (waitForAdvanceRelease)
        {
            waitForAdvanceRelease = (Keyboard.current != null && Keyboard.current.spaceKey.isPressed)
                || (Mouse.current != null && Mouse.current.leftButton.isPressed);
            return;
        }

        if (advancePressed)
        {
            if (isTyping)
            {
                FinishCurrentDialogue();
            }
            else
            {
                ShowNextDialogue();
            }
        }
    }

    private void ShowCurrentDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeDialogue(dialogueChain[dialogueIndex]));
    }

    private IEnumerator TypeDialogue(string dialogue)
    {
        isTyping = true;
        textComponent.text = "";

        foreach (char letter in dialogue)
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(timeBetweenChar);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    private void FinishCurrentDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        textComponent.text = dialogueChain[dialogueIndex];
        isTyping = false;
    }

    private void ShowNextDialogue()
    {
        dialogueIndex++;

        if (dialogueIndex >= dialogueChain.Length)
        {
            CloseDialogue();
            return;
        }

        ShowCurrentDialogue();
    }

    private void CloseDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        IsDialogueOpen = false;
        isTyping = false;
        dialogueBox.SetActive(false);
    }
}
