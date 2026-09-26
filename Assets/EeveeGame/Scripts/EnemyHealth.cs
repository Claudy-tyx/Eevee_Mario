using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 2;

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    [Header("Hit Flash")]
    [SerializeField] private float hitFlashDuration = 0.1f;

    [Header("Death")]
    [SerializeField] private int deathFlashes = 4;
    [SerializeField] private float deathFlashInterval = 0.1f;

    private int currentHealth;
    private bool isDead;

    private GameOverUI gameOverUI;
    private CrystalManager crystalManager;


    private void Awake()
    {
        currentHealth = maxHealth;
        gameOverUI = FindFirstObjectByType<GameOverUI>();
        crystalManager = FindFirstObjectByType<CrystalManager>();
    }


    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        Debug.Log(
            gameObject.name +
            " HP: " +
            currentHealth +
            "/" +
            maxHealth
        );

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        // Play hurt animation.
        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }

        StartCoroutine(HitFlash());
    }


    private IEnumerator HitFlash()
    {
        if (spriteRenderer == null)
            yield break;

        Color originalColor = spriteRenderer.color;

        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(hitFlashDuration);

        spriteRenderer.color = originalColor;
    }


    private void Die()
    {
        if (isDead)
            return;

        isDead = true;
        if (gameOverUI != null)
        {
            gameOverUI.AddEnemyDefeated();
        }

        if (crystalManager != null)
        {
            crystalManager.RegisterKill();
        }

        // Stop Caterpie movement.
        CaterpieController caterpieController =
            GetComponent<CaterpieController>();

        if (caterpieController != null)
        {
            caterpieController.enabled = false;
        }

        // Stop Butterfree movement.
        ButterfreeController butterfreeController =
            GetComponent<ButterfreeController>();

        if (butterfreeController != null)
        {
            butterfreeController.enabled = false;
        }

        EnemyContactDamage contactDamage =
            GetComponentInChildren<EnemyContactDamage>(true);

        if (contactDamage != null)
        {
            contactDamage.DisableDamage();
        }

        if (contactDamage != null)
        {
            contactDamage.DisableDamage();
        }

        // Disable Caterpie's physical body collider.
        Collider2D bodyCollider =
            GetComponent<Collider2D>();

        if (bodyCollider != null)
        {
            bodyCollider.enabled = false;
        }

        // Stop physics movement.
        Rigidbody2D rb =
            GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        // Only the Sprite remains temporarily.
        StartCoroutine(DeathFlash());
    }


    private IEnumerator DeathFlash()
    {
        if (spriteRenderer == null)
        {
            Destroy(gameObject);
            yield break;
        }

        for (int i = 0; i < deathFlashes; i++)
        {
            spriteRenderer.enabled = false;

            yield return new WaitForSeconds(
                deathFlashInterval
            );

            spriteRenderer.enabled = true;

            yield return new WaitForSeconds(
                deathFlashInterval
            );
        }

        Destroy(gameObject);
    }
}