using UnityEngine;

public class FlowerTrigger : MonoBehaviour
{
    [Header("Flower Effect")]
    [SerializeField] private float burstInterval = 0.35f;

    [Header("Sound Effect")]
    [SerializeField] private AudioClip grassSound;
    [SerializeField] private float soundVolume = 0.5f;

    private PlayerController player;
    private float nextBurstTime;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController foundPlayer =
            other.GetComponentInParent<PlayerController>();

        if (foundPlayer != null)
        {
            player = foundPlayer;

            // Immediate particles and grass sound when Eevee enters.
            PlayFlowerEffect();

            nextBurstTime = Time.time + burstInterval;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerController stayingPlayer =
            other.GetComponentInParent<PlayerController>();

        // Ignore Caterpie, Butterfree, projectiles, etc.
        if (stayingPlayer == null || stayingPlayer != player)
            return;

        if (Time.time >= nextBurstTime)
        {
            PlayFlowerEffect();

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

    private void PlayFlowerEffect()
    {
        player.PlayFlowerParticles();

        if (grassSound != null)
        {
            AudioSource.PlayClipAtPoint(
                grassSound,
                player.transform.position,
                soundVolume
            );
        }
    }
}