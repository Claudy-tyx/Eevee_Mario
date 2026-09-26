using UnityEngine;

public class MurkrowController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("Wall Detection")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance = 0.4f;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    [Header("Stuck Detection")]
    [SerializeField] private float stuckCheckTime = 0.1f;
    [SerializeField] private float minimumMovement = 0.01f;

    private Vector2 lastStuckCheckPosition;
    private float stuckTimer;

    private Rigidbody2D rb;

    // 1 = right, -1 = left
    private int direction = 1;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void Start()
    {
        // Randomly start flying left or right.
        direction = Random.value < 0.5f ? -1 : 1;

        UpdateFacing();

        if (animator != null)
        {
            animator.SetBool("Walking", true);
        }

        lastStuckCheckPosition = rb.position;
        stuckTimer = stuckCheckTime;
    }


    private void Update()
    {
        CheckForWall();
        CheckIfStuck();
    }

    private void CheckIfStuck()
    {
        stuckTimer -= Time.deltaTime;

        if (stuckTimer > 0f)
            return;

        float distanceMoved = Vector2.Distance(
            rb.position,
            lastStuckCheckPosition
        );

        if (distanceMoved < minimumMovement)
        {
            TurnAround();
        }

        lastStuckCheckPosition = rb.position;
        stuckTimer = stuckCheckTime;
    }


    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            direction * moveSpeed,
            0f
        );
    }


    private void CheckForWall()
    {
        if (wallCheck == null)
            return;

        Vector2 checkDirection =
            direction == 1
                ? Vector2.right
                : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(
            wallCheck.position,
            checkDirection,
            wallCheckDistance,
            obstacleLayer
        );

        if (hit.collider != null)
        {
            TurnAround();
        }
    }


    private void TurnAround()
    {
        direction *= -1;

        UpdateFacing();

        lastStuckCheckPosition = rb.position;
        stuckTimer = stuckCheckTime;
    }


    private void UpdateFacing()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = direction < 0;
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (wallCheck == null)
            return;

        Vector2 checkDirection =
            direction == 1
                ? Vector2.right
                : Vector2.left;

        Gizmos.DrawLine(
            wallCheck.position,
            (Vector2)wallCheck.position +
            checkDirection * wallCheckDistance
        );
    }
}