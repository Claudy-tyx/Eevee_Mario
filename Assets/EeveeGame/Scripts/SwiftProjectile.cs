using UnityEngine;
using UnityEngine.Rendering.Universal;

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

    [Header("Sound")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private float hitSoundVolume = 1f;

    [Header("Homing")]
    [SerializeField] private float homingRange = 6f;
    [SerializeField] private float homingTurnSpeed = 180f;

    [Header("Dark Map Light")]
    [SerializeField] private float lightIntensity = 1.2f;
    [SerializeField] private float lightInnerRadius = 0.5f;
    [SerializeField] private float lightOuterRadius = 2.5f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D projectileCollider;

    private bool homingEnabled = false;
    private Transform homingTarget;

    private Vector2 originalColliderOffset;

    private int damage = 1;
    private float currentSpeed;
    private float currentLifetime;

    private float originalFacingDirection;


   private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        projectileCollider = GetComponent<CircleCollider2D>();

        if (projectileCollider != null)
        {
            originalColliderOffset = projectileCollider.offset;
        }

        // Give Swift a light only in dark maps.
        if (DarkMapLighting.IsActive)
        {
            Light2D swiftLight =
                gameObject.AddComponent<Light2D>();

            swiftLight.lightType =
                Light2D.LightType.Point;

            swiftLight.intensity =
                lightIntensity;

            swiftLight.pointLightInnerRadius =
                lightInnerRadius;

            swiftLight.pointLightOuterRadius =
                lightOuterRadius;
        }
    }


    private void Start()
    {
        // Fallback values in case the projectile
        // wasn't configured by PlayerController.
        if (currentSpeed <= 0f)
            currentSpeed = speed;

        if (currentLifetime <= 0f)
            currentLifetime = lifetime;

        Destroy(gameObject, currentLifetime);
    }


    public void SetDirection(Vector2 direction)
    {
        if (direction.sqrMagnitude <= 0.001f)
            direction = Vector2.right;

        direction.Normalize();

        float moveSpeed =
            currentSpeed > 0f
            ? currentSpeed
            : speed;

        rb.linearVelocity =
            direction * moveSpeed;

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = false;
        }

        float angle =
            Mathf.Atan2(
                direction.y,
                direction.x
            ) * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(
                0f,
                0f,
                angle
            );
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
                enemy.TakeDamage(damage);
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

    public void Setup(
        Vector2 direction,
        int newDamage,
        float speedMultiplier,
        float lifetimeMultiplier,
        bool newHomingEnabled
    )
    {
        damage = newDamage;

        homingEnabled = newHomingEnabled;

        currentSpeed =
            speed * speedMultiplier;

        currentLifetime =
            lifetime * lifetimeMultiplier;

        originalFacingDirection =
            direction.x >= 0f ? 1f : -1f;

        SetDirection(direction);
    }

    private void FixedUpdate()
    {
        if (!homingEnabled)
            return;

        if (homingTarget == null)
            FindHomingTarget();

        if (homingTarget == null)
            return;

        Vector2 currentDirection =
            rb.linearVelocity.normalized;

        Vector2 targetDirection =
            ((Vector2)homingTarget.position -
            rb.position).normalized;

        float currentAngle =
            Mathf.Atan2(
                currentDirection.y,
                currentDirection.x
            ) * Mathf.Rad2Deg;

        float targetAngle =
            Mathf.Atan2(
                targetDirection.y,
                targetDirection.x
            ) * Mathf.Rad2Deg;

        float newAngle =
            Mathf.MoveTowardsAngle(
                currentAngle,
                targetAngle,
                homingTurnSpeed *
                Time.fixedDeltaTime
            );

        Vector2 newDirection =
            new Vector2(
                Mathf.Cos(newAngle * Mathf.Deg2Rad),
                Mathf.Sin(newAngle * Mathf.Deg2Rad)
            );

        rb.linearVelocity =
            newDirection * currentSpeed;

        transform.rotation =
            Quaternion.Euler(
                0f,
                0f,
                newAngle
            );
    }

    private void FindHomingTarget()
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                transform.position,
                homingRange,
                enemyLayers
            );

        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            EnemyHealth enemy =
                hit.GetComponentInParent<EnemyHealth>();

            if (enemy == null)
                continue;

            Vector2 toEnemy =
                (Vector2)enemy.transform.position -
                rb.position;

            // Only target enemies on the side
            // Eevee originally fired toward.
            if (toEnemy.x * originalFacingDirection <= 0f)
                continue;

            float distance =
                toEnemy.sqrMagnitude;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget =
                    enemy.transform;
            }
        }

        homingTarget = closestTarget;
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

        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(
                hitSound,
                transform.position,
                hitSoundVolume
            );
        }

        Destroy(gameObject);
    }
}