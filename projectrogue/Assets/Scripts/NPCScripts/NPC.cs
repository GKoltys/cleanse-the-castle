using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class NPC : MonoBehaviour, IInteractable
{
    public GameObject interactionIcon;
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    public GameObject lastInteractor;

    private void Awake()
    {
        interactionIcon.SetActive(false);
    }

    public void Interact(GameObject interactor)
    {
        lastInteractor = interactor;
        if(dialogueData == null)
        {
            return;
        }
        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    public bool CanInteract()
    {
        return dialogueData != null;
    }

    public void ShowCanInteract(bool show)
    {
        interactionIcon.SetActive(show);
    }

    // starts dialogue from first line, sets npc name, portrait and displays text panel
    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;

        lastInteractor.GetComponent<PlayerMovement>().SetCanMove(false);

        dialoguePanel.SetActive(true);
        StartCoroutine(Typeline());

    }

    // when player interacts during dialogue either instantly complete current line or go to next
    void NextLine()
    {
        if(isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }
        else if(++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(Typeline());
        }
        else
        {
            EndDialogue();
        }
    }

    // coroutine to type each letter in sentence
    IEnumerator Typeline()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach(char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);

        if (lastInteractor != null)
            lastInteractor.GetComponent<PlayerMovement>().SetCanMove(true);
    }

}
