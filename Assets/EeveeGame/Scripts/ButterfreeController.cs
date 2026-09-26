using UnityEngine;

public class ButterfreeController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1.5f;

    [Header("Roaming Area")]
    [SerializeField] private Vector2 roamAreaSize = new Vector2(5f, 3f);
    [SerializeField] private float destinationTolerance = 0.1f;

    [Header("Random Idle")]
    [SerializeField] private float minIdleTime = 0.5f;
    [SerializeField] private float maxIdleTime = 2f;

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;

    private Vector2 roamCenter;
    private Vector2 targetPosition;

    private bool isFlying = false;
    private float idleTimer;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void Start()
    {
        // Butterfree's starting position becomes
        // the centre of its roaming area.
        roamCenter = transform.position;

        StartIdle();
    }


    private void Update()
    {
        if (isFlying)
        {
            // Check whether Butterfree has reached
            // its current destination.
            if (Vector2.Distance(
                rb.position,
                targetPosition
            ) <= destinationTolerance)
            {
                StartIdle();
            }
        }
        else
        {
            idleTimer -= Time.deltaTime;

            if (idleTimer <= 0f)
            {
                ChooseNewDestination();
            }
        }

        if (animator != null)
        {
            animator.SetBool("Walking", isFlying);
        }
    }


    private void FixedUpdate()
    {
        if (!isFlying)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction =
            (targetPosition - rb.position).normalized;

        rb.linearVelocity =
            direction * moveSpeed;

        UpdateFacing(direction.x);
    }


    private void ChooseNewDestination()
    {
        float randomX = Random.Range(
            -roamAreaSize.x / 2f,
            roamAreaSize.x / 2f
        );

        float randomY = Random.Range(
            -roamAreaSize.y / 2f,
            roamAreaSize.y / 2f
        );

        targetPosition =
            roamCenter + new Vector2(randomX, randomY);

        isFlying = true;
    }


    private void StartIdle()
    {
        isFlying = false;

        rb.linearVelocity = Vector2.zero;

        idleTimer = Random.Range(
            minIdleTime,
            maxIdleTime
        );
    }


    private void UpdateFacing(float horizontalDirection)
    {
        if (spriteRenderer == null)
            return;

        if (horizontalDirection < -0.01f)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontalDirection > 0.01f)
        {
            spriteRenderer.flipX = false;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Vector3 center;

        if (Application.isPlaying)
        {
            center = roamCenter;
        }
        else
        {
            center = transform.position;
        }

        Gizmos.DrawWireCube(
            center,
            new Vector3(
                roamAreaSize.x,
                roamAreaSize.y,
                0f
            )
        );
    }
}