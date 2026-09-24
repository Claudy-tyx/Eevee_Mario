using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private bool canDamage = true;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!canDamage)
            return;

        PlayerHealth player =
            other.GetComponentInParent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }

    public void DisableDamage()
    {
        canDamage = false;
    }
}