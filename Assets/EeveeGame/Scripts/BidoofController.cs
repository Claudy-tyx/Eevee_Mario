using UnityEngine;

public class BidoofController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1.5f;

    [Header("Random Roaming")]
    [SerializeField] private float minWalkTime = 1.5f;
    [SerializeField] private float maxWalkTime = 4f;

    [SerializeField] private float minIdleTime = 0.5f;
    [SerializeField] private float maxIdleTime = 2f;

    [Header("Ground Detection")]
    [SerializeField] private Transform edgeCheck;
    [SerializeField] private float edgeCheckForwardDistance = 0.4f;
    [SerializeField] private float edgeCheckDownDistance = 0.6f;
    [SerializeField] private LayerMask groundLayer;

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;

    private int direction = 1;
    private bool isWalking = true;

    private float stateTimer;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void Start()
    {
        // Randomly start facing left or right.
        direction = Random.value < 0.5f ? -1 : 1;

        UpdateFacing();

        StartWalking();
    }


    private void Update()
    {
        stateTimer -= Time.deltaTime;

        if (isWalking)
        {
            CheckForEdge();

            if (stateTimer <= 0f)
            {
                StartIdle();
            }
        }
        else
        {
            if (stateTimer <= 0f)
            {
                // Randomly choose a new direction.
                direction = Random.value < 0.5f ? -1 : 1;

                UpdateFacing();
                StartWalking();
            }
        }

        if (animator != null)
        {
            animator.SetBool("Walking", isWalking);
        }
    }


    private void FixedUpdate()
    {
        if (isWalking)
        {
            rb.linearVelocity = new Vector2(
                direction * moveSpeed,
                rb.linearVelocity.y
            );
        }
        else
        {
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );
        }
    }


    private void CheckForEdge()
    {
        Vector2 rayStart = new Vector2(
            edgeCheck.position.x + (edgeCheckForwardDistance * direction),
            edgeCheck.position.y
        );

        bool groundAhead = Physics2D.Raycast(
            rayStart,
            Vector2.down,
            edgeCheckDownDistance,
            groundLayer
        );

        if (!groundAhead)
        {
            TurnAround();
        }
    }


    private void TurnAround()
    {
        direction *= -1;

        UpdateFacing();

        // Prevent repeatedly detecting the same edge.
        stateTimer = Random.Range(
            minWalkTime,
            maxWalkTime
        );
    }


    private void UpdateFacing()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = direction < 0;
        }
    }


    private void StartWalking()
    {
        isWalking = true;

        stateTimer = Random.Range(
            minWalkTime,
            maxWalkTime
        );
    }


    private void StartIdle()
    {
        isWalking = false;

        stateTimer = Random.Range(
            minIdleTime,
            maxIdleTime
        );
    }


    private void OnDrawGizmosSelected()
    {
        if (edgeCheck == null)
            return;

        Vector2 rayStart = new Vector2(
            edgeCheck.position.x + (edgeCheckForwardDistance * direction),
            edgeCheck.position.y
        );

        Gizmos.DrawLine(
            rayStart,
            rayStart + Vector2.down * edgeCheckDownDistance
        );
    }
}