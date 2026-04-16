using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class AiNarratorController : MonoBehaviour
{
    public static AiNarratorController Instance;

    private PlayerInput playerInput;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private float typingSpeed = 0.05f;

    private bool isTyping;
    private bool isActive;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        dialoguePanel.SetActive(false);
    }

    private void Start()
    {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
    }

    public void ShowDialogue(string text)
    {
        if (isActive) return;

        isActive = true;

        playerInput.actions["Attack"].Disable();
        playerInput.actions["Move"].Disable();

        nameText.SetText("???");

        dialoguePanel.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(TypeLine(text));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    public void CloseDialogue()
    {
        StopAllCoroutines();
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        isActive = false;

        playerInput.actions["Attack"].Enable();
        playerInput.actions["Move"].Enable();
    }

    private void Update()
    {
        if (!isActive) return;

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (isTyping)
            {
                StopAllCoroutines();
                isTyping = false;
            }
            else
            {
                CloseDialogue();
            }
        }
    }
}
