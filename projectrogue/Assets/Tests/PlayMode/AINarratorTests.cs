using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Reflection;

public class AiNarratorTests
{
    // assert ui shows when showdialogue is ran
    [Test]
    public void ShowDialogue_ActivatesPanelAndSetsName()
    {
        var context = CreateNarrator();

        context.Narrator.ShowDialogue("The castle watches.");

        Assert.IsTrue(context.DialoguePanel.activeSelf);
        Assert.AreEqual("???", context.NameText.text);
    }

    // assert ui is gone when closedialogue is ran
    [Test]
    public void CloseDialogue_HidesPanelAndClearsText()
    {
        var context = CreateNarrator();

        context.Narrator.ShowDialogue("The castle watches.");
        context.Narrator.CloseDialogue();

        Assert.IsFalse(context.DialoguePanel.activeSelf);
        Assert.AreEqual("", context.DialogueText.text);
    }

    // helper functions
    private static NarratorTestContext CreateNarrator()
    {
        var obj = new GameObject("AiNarratorController");
        var narrator = obj.AddComponent<AiNarratorController>();

        var panel = new GameObject("DialoguePanel");
        panel.SetActive(false);

        var dialogueText = new GameObject("DialogueText").AddComponent<TextMeshProUGUI>();
        var nameText = new GameObject("NameText").AddComponent<TextMeshProUGUI>();

        SetPrivate(narrator, "dialoguePanel", panel);
        SetPrivate(narrator, "dialogueText", dialogueText);
        SetPrivate(narrator, "nameText", nameText);
        SetPrivate(narrator, "typingSpeed", 0f);

        return new NarratorTestContext
        {
            Narrator = narrator,
            DialoguePanel = panel,
            DialogueText = dialogueText,
            NameText = nameText
        };
    }

    private static void SetPrivate(object target, string fieldName, object value)
    {
        target.GetType()
            .GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(target, value);
    }

    private class NarratorTestContext
    {
        public AiNarratorController Narrator;
        public GameObject DialoguePanel;
        public TMP_Text DialogueText;
        public TMP_Text NameText;
    }
}