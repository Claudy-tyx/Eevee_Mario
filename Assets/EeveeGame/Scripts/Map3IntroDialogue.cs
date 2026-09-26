using UnityEngine;
using UnityEngine.InputSystem;

public class Map3IntroDialogue : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private DialogueUI dialogueUI;

    [Header("Dialogue")]
    [TextArea(2, 5)]
    [SerializeField] private string introMessage =
        "This path looks really difficult... I'm going to need an extra jump to make it through. I should watch out for those Murkrow too!";

    private bool talking = false;
    private bool canAdvanceDialogue = false;


    private void Start()
    {
        // No GameManager means we cannot track
        // whether this dialogue has already been seen.
        if (GameManager.Instance == null)
        {
            Debug.LogWarning(
                "Map3IntroDialogue: GameManager not found."
            );

            return;
        }

        // Do not show the dialogue again if the player
        // has already entered Map 3 during this run.
        if (GameManager.Instance.HasSeenMap3Intro())
            return;

        ShowIntroDialogue();
    }


    private void Update()
    {
        if (!talking || !canAdvanceDialogue)
            return;

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
            EndDialogue();
        }
    }


    private void LateUpdate()
    {
        // Prevent the same input that caused the scene
        // transition from immediately closing the dialogue.
        if (talking && !canAdvanceDialogue)
        {
            canAdvanceDialogue = true;
        }
    }


    private void ShowIntroDialogue()
    {
        if (dialogueUI == null)
        {
            Debug.LogWarning(
                "Map3IntroDialogue: DialogueUI is not assigned."
            );

            return;
        }

        // Mark it immediately so this dialogue only happens
        // once during the current run.
        GameManager.Instance.MarkMap3IntroSeen();

        talking = true;
        canAdvanceDialogue = false;

        // Freeze gameplay while Eevee is talking.
        Time.timeScale = 0f;

        dialogueUI.HideChoices();

        dialogueUI.ShowDialogue(
            "Eevee",
            introMessage
        );
    }


    private void EndDialogue()
    {
        talking = false;
        canAdvanceDialogue = false;

        if (dialogueUI != null)
        {
            dialogueUI.HideDialogue();
        }

        Time.timeScale = 1f;
    }
}