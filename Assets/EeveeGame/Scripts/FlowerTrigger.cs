using UnityEngine;

public class FlowerTrigger : MonoBehaviour
{
    [SerializeField] private float burstInterval = 0.35f;

    private PlayerController player;
    private float nextBurstTime;


    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController foundPlayer =
            other.GetComponentInParent<PlayerController>();

        if (foundPlayer != null)
        {
            player = foundPlayer;

            // Immediate burst when Eevee enters.
            player.PlayFlowerParticles();

            nextBurstTime = Time.time + burstInterval;
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (player == null)
            return;

        if (Time.time >= nextBurstTime)
        {
            player.PlayFlowerParticles();

            nextBurstTime = Time.time + burstInterval;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController exitingPlayer =
            other.GetComponentInParent<PlayerController>();

        if (exitingPlayer == player)
        {
            player = null;
        }
    }
}