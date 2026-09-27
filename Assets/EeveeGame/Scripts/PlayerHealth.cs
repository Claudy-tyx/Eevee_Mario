using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 3;

    [Header("Damage")]
    [SerializeField] private float invulnerabilityTime = 1f;

    [Header("Flash")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashInterval = 0.1f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("UI")]
    [SerializeField] private HealthUI healthUI;

    [Header("Game Over")]
    [SerializeField] private GameOverUI gameOverUI;
    [SerializeField] private PlayerController playerController;

    private int currentHealth;
    private bool isInvulnerable;
    private bool isDead;

    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            maxHealth =
                GameManager.Instance.GetMaxHealth();

            currentHealth =
                GameManager.Instance.GetCurrentHealth();
        }
        else
        {
            currentHealth = maxHealth;
        }

        if (playerController == null)
        {
            playerController =
                GetComponent<PlayerController>();
        }
    }

    private void Start()
    {
        if (healthUI != null)
        {
            healthUI.SetHealth(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable || isDead)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetCurrentHealth(
                currentHealth
            );
        }

        if (healthUI != null)
        {
            healthUI.SetHealth(currentHealth, maxHealth);
        }

        Debug.Log(
            "Eevee HP: " +
            currentHealth +
            "/" +
            maxHealth
        );

        // If this hit killed Eevee, go directly to Faint.
        // Do NOT play Hurt first.
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        // Normal non-lethal damage.
        if (playerController != null)
        {
            playerController.PlayHurtSound();
        }

        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }

        StartCoroutine(InvulnerabilityFlash());
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;
        isInvulnerable = true;

        // Count this death for the current run.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterDeath();
        }

        // Play faint sound before disabling PlayerController.
        if (playerController != null)
        {
            playerController.PlayFaintSound();
        }

        // Stop Eevee controls.
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Stop Eevee's current movement.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;

            // Prevent anything from physically pushing Eevee.
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // Play faint animation.
        if (animator != null)
        {
            // Make sure Hurt cannot interfere with Faint.
            animator.ResetTrigger("Hurt");
            animator.SetTrigger("Faint");
        }

        StartCoroutine(GameOverDelay());
    }

    private IEnumerator GameOverDelay()
    {
        // Give the Faint animation time to finish.
        yield return new WaitForSeconds(1f);

        // Freeze the entire game.
        Time.timeScale = 0f;

        if (gameOverUI != null)
        {
            gameOverUI.ShowGameOver();
        }
    }

    public void Heal(int amount)
    {
        if (isDead)
            return;

        if (currentHealth >= maxHealth)
            return;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetCurrentHealth(
                currentHealth
            );
        }

        if (healthUI != null)
        {
            healthUI.SetHealth(currentHealth, maxHealth);
        }

        StartCoroutine(HealFlash());

        Debug.Log(
            "Eevee healed! HP: " +
            currentHealth +
            "/" +
            maxHealth
        );
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;

        currentHealth += amount;

        currentHealth =
            Mathf.Min(currentHealth, maxHealth);

        // Save the max health increase permanently
        // for the current run.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.IncreaseMaxHealth(amount);
        }

        if (healthUI != null)
        {
            healthUI.SetHealth(
                currentHealth,
                maxHealth
            );
        }

        Debug.Log(
            "Eevee Max HP increased! HP: " +
            currentHealth +
            "/" +
            maxHealth
        );
    }

    private IEnumerator HealFlash()
    {
        if (spriteRenderer == null)
            yield break;

        Color originalColor = spriteRenderer.color;

        spriteRenderer.color = Color.green;

        yield return new WaitForSeconds(0.15f);

        spriteRenderer.color = originalColor;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = Color.green;

        yield return new WaitForSeconds(0.15f);

        spriteRenderer.color = originalColor;
    }

    private IEnumerator InvulnerabilityFlash()
    {
        isInvulnerable = true;

        float timer = invulnerabilityTime;

        while (timer > 0f)
        {
            if (spriteRenderer != null)
                spriteRenderer.enabled = false;

            yield return new WaitForSeconds(
                flashInterval
            );

            if (spriteRenderer != null)
                spriteRenderer.enabled = true;

            yield return new WaitForSeconds(
                flashInterval
            );

            timer -= flashInterval * 2f;
        }

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        isInvulnerable = false;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}