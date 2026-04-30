using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCTests
{
    // assert npc cannot interact without dialoguedata
    [Test]
    public void CanInteract_WhenDialogueDataIsNull_ReturnsFalse()
    {
        var npc = CreateNPCWithoutDialogue();

        Assert.IsFalse(npc.CanInteract());
    }

    // assert interaction icon is shown if can interact
    [Test]
    public void ShowCanInteract_TogglesInteractionIcon()
    {
        var npc = CreateNPCWithoutDialogue();

        npc.ShowCanInteract(true);
        Assert.IsTrue(npc.interactionIcon.activeSelf);

        npc.ShowCanInteract(false);
        Assert.IsFalse(npc.interactionIcon.activeSelf);
    }

    // assert dialogue ui shows when interacting with npc
    [Test]
    public void Interact_StartsDialogue_ShowsPanelAndSetsName()
    {
        var npc = CreateNPC("Test", "Hello!");
        var player = CreatePlayer();

        npc.Interact(player);

        Assert.IsTrue(npc.dialoguePanel.activeSelf);
        Assert.AreEqual("Test", npc.nameText.text);
    }

    // assert player movement is disabled if in dialogue
    [Test]
    public void Interact_DisablesPlayerMovement()
    {
        var npc = CreateNPC("Test", "Hello!");
        var player = CreatePlayer();
        var movement = player.GetComponent<PlayerMovement>();

        npc.Interact(player);

        Assert.IsFalse(movement.CanMove());
    }

    // assert dialogue advances with multiple lines
    [Test]
    public void Interact_WhenDialogueActive_AdvancesDialogue()
    {
        var npc = CreateNPC("Test", "Line1", "Line2");
        var player = CreatePlayer();

        CompleteLine(npc, player, lineIndex: 0);
        CompleteLine(npc, player, lineIndex: 1);

        Assert.AreEqual("Line2", npc.dialogueText.text);
    }

    // assert dialogue ui disappears after dialogue is finished
    [Test]
    public void DialogueEndsAfterFinalLine_PanelCloses()
    {
        var npc = CreateNPC("Test", "OnlyLine");
        var player = CreatePlayer();

        CompleteLine(npc, player, lineIndex: 0);
        npc.Interact(player);

        Assert.IsFalse(npc.dialoguePanel.activeSelf);
    }

    // helper functions
    private static NPC CreateNPCWithoutDialogue()
    {
        var npcObj = new GameObject("NPC");
        var npc = npcObj.AddComponent<NPC>();

        npc.interactionIcon = new GameObject("InteractionIcon");

        return npc;
    }

    private static NPC CreateNPC(string npcName, params string[] lines)
    {
        var npcObj = new GameObject("NPC");
        var npc = npcObj.AddComponent<NPC>();

        npc.interactionIcon = new GameObject("InteractionIcon");

        npc.dialoguePanel = new GameObject("DialoguePanel");
        npc.dialoguePanel.SetActive(false);

        npc.nameText = new GameObject("NameText").AddComponent<TextMeshProUGUI>();
        npc.dialogueText = new GameObject("DialogueText").AddComponent<TextMeshProUGUI>();
        npc.portraitImage = new GameObject("Portrait").AddComponent<Image>();

        npc.dialogueData = ScriptableObject.CreateInstance<NPCDialogue>();
        npc.dialogueData.npcName = npcName;
        npc.dialogueData.dialogueLines = lines;
        npc.dialogueData.typingSpeed = 0f;

        return npc;
    }

    private static GameObject CreatePlayer()
    {
        var player = new GameObject("Player");

        player.AddComponent<Rigidbody2D>();
        player.AddComponent<Animator>();
        player.AddComponent<PlayerStats>();
        player.AddComponent<PlayerMovement>();

        return player;
    }

    private static void CompleteLine(NPC npc, GameObject player, int lineIndex)
    {
        npc.Interact(player);
        npc.Interact(player);

        Assert.AreEqual(
            npc.dialogueData.dialogueLines[lineIndex],
            npc.dialogueText.text
        );
    }
}