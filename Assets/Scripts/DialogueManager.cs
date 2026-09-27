using UnityEngine;
using System.Collections;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public float timeBetweenChar = 0.04f;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] TextMeshProUGUI textComponent;
    private Coroutine typingCoroutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartTyping(string dialogue)
    {
        dialogueBox.SetActive(true);
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeDialogue(dialogue));
    }

    private IEnumerator TypeDialogue(string dialogue)
    {
        textComponent.text = "";
        foreach (char letter in dialogue.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(timeBetweenChar);
        }

        yield return new WaitForSeconds(1f); 
        dialogueBox.SetActive(false);
    }
}
