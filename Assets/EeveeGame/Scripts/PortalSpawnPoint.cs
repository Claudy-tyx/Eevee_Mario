using UnityEngine;

public class PortalSpawnPoint : MonoBehaviour
{
    [SerializeField] private string spawnID;

    private void Start()
    {
        if (GameManager.Instance == null)
            return;

        string requestedSpawn =
            GameManager.Instance.GetDestinationSpawn();

        if (string.IsNullOrEmpty(requestedSpawn))
            return;

        if (requestedSpawn != spawnID)
            return;

        PlayerController player =
            FindFirstObjectByType<PlayerController>();

        if (player == null)
            return;

        player.transform.position =
            transform.position;

        GameManager.Instance.ClearDestinationSpawn();
    }
}