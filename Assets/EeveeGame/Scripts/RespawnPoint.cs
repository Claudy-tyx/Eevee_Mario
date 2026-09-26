using System.Collections;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private GameObject prefabToSpawn;

    [Header("Respawn")]
    [SerializeField] private float respawnTime = 8f;

    [Header("Spawn Warning")]
    [SerializeField] private bool useSpawnWarning = true;
    [SerializeField] private Sprite warningSprite;
    [SerializeField] private float warningTime = 2f;
    [SerializeField] private float flashInterval = 0.15f;

    [Header("Starting Object")]
    [SerializeField] private bool spawnOnStart = true;

    private GameObject currentObject;
    private bool waitingToRespawn;

    private SpriteRenderer warningRenderer;

    private void Awake()
    {
        // Only create a warning renderer for spawn points
        // that actually use the warning effect.
        if (!useSpawnWarning)
            return;

        // Reuse an existing SpriteRenderer if there is one.
        warningRenderer = GetComponent<SpriteRenderer>();

        if (warningRenderer == null)
        {
            warningRenderer =
                gameObject.AddComponent<SpriteRenderer>();
        }

        // Copy sorting settings from the spawned prefab.
        if (prefabToSpawn != null)
        {
            SpriteRenderer prefabRenderer =
                prefabToSpawn.GetComponentInChildren<SpriteRenderer>();

            if (prefabRenderer != null)
            {
                warningRenderer.sortingLayerName =
                    prefabRenderer.sortingLayerName;

                warningRenderer.sortingOrder =
                    prefabRenderer.sortingOrder;
            }
        }

        warningRenderer.sprite = warningSprite;
        warningRenderer.enabled = false;
    }

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnObject();
        }
    }

    private void Update()
    {
        if (currentObject == null &&
            !waitingToRespawn)
        {
            StartCoroutine(Respawn());
        }
    }

    private void SpawnObject()
    {
        if (prefabToSpawn == null)
            return;

        currentObject = Instantiate(
            prefabToSpawn,
            transform.position,
            transform.rotation
        );
    }

    private IEnumerator Respawn()
    {
        waitingToRespawn = true;

        // Normal respawn delay.
        yield return new WaitForSeconds(respawnTime);

        // Flash before spawning.
        if (useSpawnWarning &&
            warningRenderer != null &&
            warningSprite != null)
        {
            float timer = warningTime;

            while (timer > 0f)
            {
                warningRenderer.enabled = true;

                yield return new WaitForSeconds(
                    flashInterval
                );

                warningRenderer.enabled = false;

                yield return new WaitForSeconds(
                    flashInterval
                );

                timer -= flashInterval * 2f;
            }

            warningRenderer.enabled = false;
        }

        // Enemy only exists AFTER warning.
        SpawnObject();

        waitingToRespawn = false;
    }
}