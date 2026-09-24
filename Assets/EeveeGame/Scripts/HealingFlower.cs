using UnityEngine;

public class HealingFlower : MonoBehaviour
{
    [SerializeField] private int healAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player =
            other.GetComponentInParent<PlayerHealth>();

        if (player == null)
            return;

        // Don't collect it if Eevee already has full HP.
        if (player.GetCurrentHealth() >= player.GetMaxHealth())
            return;

        // Heal Eevee.
        player.Heal(healAmount);

        // Remove the flower.
        Destroy(gameObject);
    }
}