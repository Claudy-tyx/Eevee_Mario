using UnityEngine;
using UnityEngine.InputSystem;

public class SylveonNPC : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactionDistance = 2.5f;

    [Header("Dialogue")]
    [SerializeField] private DialogueUI dialogueUI;

    [Header("Heart Exchange")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private int nuggetCost = 2;

    private bool introductionSeen = false;
    private bool waitingForChoice = false;
    private bool endingDialogue = false;

    private bool talking = false;
    private int dialogueIndex = 0;
    private bool canAdvanceDialogue = false;

    private void Start()
    {
        if (dialogueUI != null)
        {
            dialogueUI.GetGiveButton().onClick.AddListener(
                GiveNuggets
            );

            dialogueUI.GetNotNowButton().onClick.AddListener(
                DeclineTrade
            );
        }
    }

    private string[] speakers =
    {
        "Sylveon",
        "Eevee",
        "Sylveon"
    };

    private string[] dialogue =
    {
        "Oh! Hello, Eevee!",
        "Eevee!",
        "I've been collecting Gold Nuggets! If you bring me 2 of them, I can give you an extra heart."
    };

    private void Update()
    {
        if (!talking ||
            !canAdvanceDialogue ||
            waitingForChoice)
        {
            return;
        }

        bool continuePressed = false;

        if (Keyboard.current != null &&
            Keyboard.current.anyKey.wasPressedThisFrame)
        {
            continuePressed = true;
        }

        if (Mouse.current != null &&
            (Mouse.current.leftButton.wasPressedThisFrame ||
            Mouse.current.rightButton.wasPressedThisFrame))
        {
            continuePressed = true;
        }

        if (continuePressed)
        {
            NextDialogue();
        }
    }

    private void LateUpdate()
    {
        if (talking && !canAdvanceDialogue)
        {
            canAdvanceDialogue = true;
        }
    }

    private void OnMouseDown()
    {
        if (talking)
            return;

        if (player == null)
            return;

        float distance =
            Vector2.Distance(
                transform.position,
                player.position
            );

        if (distance > interactionDistance)
            return;

        StartDialogue();
    }

    private void StartDialogue()
    {
        talking = true;
        canAdvanceDialogue = false;

        Time.timeScale = 0f;

        if (!introductionSeen)
        {
            dialogueIndex = 0;
            ShowCurrentDialogue();
        }
        else
        {
            ShowTradeChoice();
        }
    }

    private void DeclineTrade()
    {
        if (!waitingForChoice)
            return;

        waitingForChoice = false;

        dialogueUI.HideChoices();

        dialogueUI.ShowDialogue(
            "Sylveon",
            "No problem! Come back whenever you find some."
        );

        endingDialogue = true;
    }

    private void NextDialogue()
    {
        if (endingDialogue)
        {
            EndDialogue();
            return;
        }

        dialogueIndex++;

        if (dialogueIndex >= dialogue.Length)
        {
            introductionSeen = true;
            ShowTradeChoice();
            return;
        }

        ShowCurrentDialogue();
    }

    private void GiveNuggets()
    {
        if (!waitingForChoice)
            return;

        dialogueUI.HideChoices();
        waitingForChoice = false;

        if (GameManager.Instance == null)
        {
            EndDialogue();
            return;
        }

        if (GameManager.Instance.SpendGold(nuggetCost))
        {
            if (playerHealth != null)
                playerHealth.IncreaseMaxHealth(1);

            dialogueUI.ShowDialogue(
                "Sylveon",
                "There you go! You should feel a little stronger now!"
            );
        }
        else
        {
            dialogueUI.ShowDialogue(
                "Sylveon",
                "You don't have enough Gold Nuggets yet. Come back when you've found 2!"
            );
        }

        endingDialogue = true;
    }

    private void ShowTradeChoice()
    {
        waitingForChoice = true;

        dialogueUI.ShowDialogue(
            "Sylveon",
            "Would you like to give me 2 Gold Nuggets for an extra heart?"
        );

        dialogueUI.ShowChoices();
    }

    private void ShowCurrentDialogue()
    {
        dialogueUI.ShowDialogue(
            speakers[dialogueIndex],
            dialogue[dialogueIndex]
        );
    }

    private void EndDialogue()
    {
        talking = false;
        canAdvanceDialogue = false;
        waitingForChoice = false;
        endingDialogue = false;

        dialogueUI.HideDialogue();

        Time.timeScale = 1f;
    }
}