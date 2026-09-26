using UnityEngine;
using UnityEngine.InputSystem;

public class MapUnlockDialogue : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private DialogueUI dialogueUI;

    [Header("Map 2 Dialogue")]
    [SerializeField] private string map2Message =
        "Oh! I think a path opened up!";

    [Header("Map 3 Dialogue")]
    [SerializeField] private string map3Message =
        "Oh! I think another path opened up!";

    private bool talking = false;
    private bool canAdvanceDialogue = false;
    private bool subscribed = false;


    private void Start()
    {
        SubscribeToEvents();
    }


    // =========================
    // EVENT SUBSCRIPTION
    // =========================

    private void SubscribeToEvents()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning(
                "MapUnlockDialogue: GameManager not found."
            );

            return;
        }

        GameManager.Instance.OnMap2Unlocked +=
            ShowMap2Dialogue;

        GameManager.Instance.OnMap3Unlocked +=
            ShowMap3Dialogue;

        subscribed = true;

        Debug.Log(
            "MapUnlockDialogue subscribed to map unlock events."
        );
    }


    private void OnDestroy()
    {
        if (!subscribed)
            return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnMap2Unlocked -=
                ShowMap2Dialogue;

            GameManager.Instance.OnMap3Unlocked -=
                ShowMap3Dialogue;
        }
    }


    // =========================
    // INPUT
    // =========================

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
        if (talking && !canAdvanceDialogue)
        {
            canAdvanceDialogue = true;
        }
    }


    // =========================
    // MAP 2 UNLOCK
    // =========================

    private void ShowMap2Dialogue()
    {
        Debug.Log(
            "MapUnlockDialogue received Map 2 unlock event."
        );

        ShowUnlockDialogue(map2Message);
    }


    // =========================
    // MAP 3 UNLOCK
    // =========================

    private void ShowMap3Dialogue()
    {
        Debug.Log(
            "MapUnlockDialogue received Map 3 unlock event."
        );

        ShowUnlockDialogue(map3Message);
    }


    // =========================
    // SHOW DIALOGUE
    // =========================

    private void ShowUnlockDialogue(string message)
    {
        if (dialogueUI == null)
        {
            Debug.LogWarning(
                "MapUnlockDialogue: DialogueUI is not assigned."
            );

            return;
        }

        talking = true;
        canAdvanceDialogue = false;

        // Freeze gameplay while Eevee is talking.
        Time.timeScale = 0f;

        dialogueUI.HideChoices();

        dialogueUI.ShowDialogue(
            "Eevee",
            message
        );
    }


    // =========================
    // END DIALOGUE
    // =========================

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