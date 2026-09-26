using UnityEngine;
using UnityEngine.InputSystem;

public class RibbonCollectible : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private CompletionUI completionUI;

    [Header("Dialogue")]
    [TextArea(2, 4)]
    [SerializeField] private string ribbonMessage =
        "Yay! I found my ribbon!";

    private bool collected = false;
    private bool canAdvanceDialogue = false;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected)
            return;

        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player == null)
            return;

        CollectRibbon();
    }


    private void CollectRibbon()
    {
        collected = true;

        // Stop the completion timer immediately.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CompleteRun();
        }

        // Hide the ribbon after collecting it.
        SpriteRenderer spriteRenderer =
            GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        Collider2D ribbonCollider =
            GetComponent<Collider2D>();

        if (ribbonCollider != null)
        {
            ribbonCollider.enabled = false;
        }

        // Freeze gameplay.
        Time.timeScale = 0f;

        if (dialogueUI != null)
        {
            dialogueUI.HideChoices();

            dialogueUI.ShowDialogue(
                "Eevee",
                ribbonMessage
            );
        }

        canAdvanceDialogue = false;
    }


    private void Update()
    {
        if (!collected || !canAdvanceDialogue)
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
            FinishDialogue();
        }
    }


    private void LateUpdate()
    {
        if (collected && !canAdvanceDialogue)
        {
            canAdvanceDialogue = true;
        }
    }


    private void FinishDialogue()
    {
        canAdvanceDialogue = false;

        if (dialogueUI != null)
        {
            dialogueUI.HideDialogue();
        }

        if (completionUI != null)
        {
            completionUI.ShowCompletion();
        }

        // Keep gameplay frozen while
        // the results screen is displayed.
        Time.timeScale = 0f;
    }
}