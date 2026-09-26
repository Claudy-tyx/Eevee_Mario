using UnityEngine;

public class LeafSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera targetCamera;
    [SerializeField] private GameObject leafPrefab;

    [Header("Leaf Sprites")]
    [SerializeField] private Sprite[] leafSprites;

    [Header("Spawn Timing")]
    [SerializeField] private float minSpawnTime = 0.8f;
    [SerializeField] private float maxSpawnTime = 1.5f;

    [Header("Performance")]
    [SerializeField] private int maxLeaves = 10;
    [SerializeField] private float leafLifetime = 10f;

    [Header("Spawn Area")]
    [SerializeField] private float horizontalPadding = 1f;
    [SerializeField] private float verticalOffset = 1f;

    [Header("Leaf Size")]
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.2f;

    private float spawnTimer;
    private int activeLeaves;

    private void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        ResetSpawnTimer();
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            if (activeLeaves < maxLeaves)
                SpawnLeaf();

            ResetSpawnTimer();
        }
    }

    private void SpawnLeaf()
    {
        if (targetCamera == null ||
            leafPrefab == null ||
            leafSprites == null ||
            leafSprites.Length == 0)
        {
            return;
        }

        float cameraHeight =
            targetCamera.orthographicSize * 2f;

        float cameraWidth =
            cameraHeight * targetCamera.aspect;

        float left =
            targetCamera.transform.position.x -
            cameraWidth / 2f;

        float right =
            targetCamera.transform.position.x +
            cameraWidth / 2f;

        float top =
            targetCamera.transform.position.y +
            cameraHeight / 2f;

        Vector3 spawnPosition = new Vector3(
            Random.Range(
                left - horizontalPadding,
                right + horizontalPadding
            ),
            top + verticalOffset,
            0f
        );

        GameObject leaf = Instantiate(
            leafPrefab,
            spawnPosition,
            Quaternion.identity
        );

        activeLeaves++;

        SpriteRenderer renderer =
            leaf.GetComponent<SpriteRenderer>();

        if (renderer != null)
        {
            renderer.sprite =
                leafSprites[
                    Random.Range(0, leafSprites.Length)
                ];
        }

        float randomScale =
            Random.Range(minScale, maxScale);

        leaf.transform.localScale =
            Vector3.one * randomScale;

        leaf.transform.rotation =
            Quaternion.Euler(
                0f,
                0f,
                Random.Range(0f, 360f)
            );

        // Let the leaf report back when it disappears.
        LeafLifetime lifetime =
            leaf.GetComponent<LeafLifetime>();

        if (lifetime == null)
            lifetime = leaf.AddComponent<LeafLifetime>();

        lifetime.Setup(this, leafLifetime);
    }

    public void LeafDestroyed()
    {
        activeLeaves = Mathf.Max(0, activeLeaves - 1);
    }

    private void ResetSpawnTimer()
    {
        spawnTimer =
            Random.Range(
                minSpawnTime,
                maxSpawnTime
            );
    }
}