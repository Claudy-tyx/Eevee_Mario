using UnityEngine;

public class GoldNugget : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player == null)
            return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddGold(1);
        }

        Destroy(gameObject);
    }
}