using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text speakerName;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Choices")]
    [SerializeField] private GameObject choiceContainer;
    [SerializeField] private Button giveButton;
    [SerializeField] private Button notNowButton;

    private void Start()
    {
        if (choiceContainer != null)
            choiceContainer.SetActive(false);

        dialoguePanel.SetActive(false);
    }

    public void ShowChoices()
    {
        if (choiceContainer != null)
            choiceContainer.SetActive(true);
    }

    public void HideChoices()
    {
        if (choiceContainer != null)
            choiceContainer.SetActive(false);
    }

    public Button GetGiveButton()
    {
        return giveButton;
    }

    public Button GetNotNowButton()
    {
        return notNowButton;
    }

    public void ShowDialogue(
        string speaker,
        string message
    )
    {
        speakerName.text = speaker;
        dialogueText.text = message;

        dialoguePanel.SetActive(true);
    }

    public void HideDialogue()
    {
        HideChoices();
        dialoguePanel.SetActive(false);
    }
}