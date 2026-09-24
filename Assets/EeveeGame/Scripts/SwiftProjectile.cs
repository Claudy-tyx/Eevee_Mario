using UnityEngine;

public class SwiftProjectile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 8f;

    [Header("Lifetime")]
    [SerializeField] private float lifetime = 3f;

    [Header("Collision")]
    [SerializeField] private LayerMask obstacleLayers;
    [SerializeField] private LayerMask enemyLayers;

    [Header("Hit Effect")]
    [SerializeField] private GameObject hitEffectPrefab;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D projectileCollider;

    private Vector2 originalColliderOffset;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        projectileCollider = GetComponent<CircleCollider2D>();

        if (projectileCollider != null)
        {
            originalColliderOffset = projectileCollider.offset;
        }
    }


    private void Start()
    {
        Destroy(gameObject, lifetime);
    }


    public void SetDirection(bool facingRight)
    {
        float direction = facingRight ? 1f : -1f;

        rb.linearVelocity = new Vector2(
            direction * speed,
            0f
        );

        spriteRenderer.flipX = !facingRight;

        if (projectileCollider != null)
        {
            projectileCollider.offset = new Vector2(
                Mathf.Abs(originalColliderOffset.x) * direction,
                originalColliderOffset.y
            );
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore Eevee.
        if (other.GetComponentInParent<PlayerController>() != null)
            return;

        int otherLayer = other.gameObject.layer;

        bool hitObstacle =
            (obstacleLayers.value & (1 << otherLayer)) != 0;

        bool hitEnemy =
            (enemyLayers.value & (1 << otherLayer)) != 0;


        // Hit an enemy.
        if (hitEnemy)
        {
            EnemyHealth enemy =
                other.GetComponentInParent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }

            HitSomething();
            return;
        }


        // Hit a wall/platform.
        if (hitObstacle)
        {
            HitSomething();
        }
    }


    private void HitSomething()
    {
        if (hitEffectPrefab != null)
        {
            Instantiate(
                hitEffectPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        Destroy(gameObject);
    }
}