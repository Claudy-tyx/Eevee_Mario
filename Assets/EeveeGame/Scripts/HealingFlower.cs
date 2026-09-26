using UnityEngine;

public class HealingFlower : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth =
            other.GetComponentInParent<PlayerHealth>();

        if (playerHealth == null)
            return;

        // Don't consume flower at full HP.
        if (playerHealth.GetCurrentHealth() >=
            playerHealth.GetMaxHealth())
        {
            return;
        }

        PlayerStats playerStats =
            other.GetComponentInParent<PlayerStats>();

        int healAmount = 1;

        if (playerStats != null)
        {
            healAmount =
                playerStats.GetFlowerHealAmount();
        }

        playerHealth.Heal(healAmount);

        Destroy(gameObject);
    }
}