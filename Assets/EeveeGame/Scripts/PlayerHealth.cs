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

    private bool isDead;

    private int currentHealth;
    private bool isInvulnerable;


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        if (healthUI != null)
        {
            healthUI.SetHealth(currentHealth);
        }
    }


    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (healthUI != null)
        {
            healthUI.SetHealth(currentHealth);
        }

        Debug.Log(
            "Eevee HP: " +
            currentHealth +
            "/" +
            maxHealth
        );

        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(InvulnerabilityFlash());
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;
        isInvulnerable = true;

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

            // Prevent enemies from physically pushing Eevee.
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // Play faint animation.
        if (animator != null)
        {
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
        if (currentHealth >= maxHealth)
            return;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        if (healthUI != null)
        {
            healthUI.SetHealth(currentHealth);
        }

        StartCoroutine(HealFlash());

        Debug.Log(
            "Eevee healed! HP: " +
            currentHealth +
            "/" +
            maxHealth
        );
    }

    private IEnumerator HealFlash()
    {
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
            spriteRenderer.enabled = false;

            yield return new WaitForSeconds(
                flashInterval
            );

            spriteRenderer.enabled = true;

            yield return new WaitForSeconds(
                flashInterval
            );

            timer -= flashInterval * 2f;
        }

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