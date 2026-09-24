using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckRadius = 0.15f;

    [Header("Tackle")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private float attackCooldown = 0.5f;

    [Header("Swift")]
    [SerializeField] private Transform projectilePoint;
    [SerializeField] private GameObject swiftProjectilePrefab;
    [SerializeField] private float swiftCooldown = 0.4f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem flowerParticles;

    private Rigidbody2D rb;

    private float horizontal;
    private bool isGrounded;
    private bool facingRight = true;

    private float nextAttackTime;
    private float nextSwiftTime;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Automatically find these on the Sprite child
        // if they weren't assigned manually.
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }


    private void Update()
    {
        CheckGround();
        HandleMovementInput();
        HandleJump();
        HandleAttacks();
        UpdateAnimations();
        UpdateFacingPoints();
    }


    private void CheckGround()
    {
        if (groundCheck == null)
            return;

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }


    private void HandleMovementInput()
    {
        horizontal = 0f;

        if (Keyboard.current == null)
            return;

        // Move left
        if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
        {
            horizontal = -1f;
            facingRight = false;

            if (spriteRenderer != null)
                spriteRenderer.flipX = true;
        }

        if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            horizontal = 1f;
            facingRight = true;

            if (spriteRenderer != null)
                spriteRenderer.flipX = false;
        }
    }


    private void HandleJump()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );
        }
    }

    public void PlayFlowerParticles()
    {
        if (flowerParticles != null)
        {
            flowerParticles.Play();
        }
    }

    public void SpawnSwift()
    {
        if (swiftProjectilePrefab == null ||
            projectilePoint == null)
            return;

        GameObject projectile = Instantiate(
            swiftProjectilePrefab,
            projectilePoint.position,
            Quaternion.identity
        );

        SwiftProjectile swift =
            projectile.GetComponent<SwiftProjectile>();

        if (swift != null)
        {
            swift.SetDirection(facingRight);
        }
    }


    private void HandleAttacks()
    {
        if (Keyboard.current == null)
            return;

        // J = Tackle
        if (Keyboard.current.jKey.wasPressedThisFrame &&
            Time.time >= nextAttackTime)
        {
            Attack();
        }

        // K = Swift
        if (Keyboard.current.kKey.wasPressedThisFrame &&
            Time.time >= nextSwiftTime)
        {
            ShootSwift();
        }
    }


    private void Attack()
    {
        nextAttackTime = Time.time + attackCooldown;

        if (animator != null)
            animator.SetTrigger("Attack");
    }


    private void ShootSwift()
    {
        nextSwiftTime = Time.time + swiftCooldown;

        if (animator != null)
            animator.SetTrigger("Shoot");
    }


    private void UpdateAnimations()
    {
        if (animator == null)
            return;

        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        animator.SetBool("Grounded", isGrounded);
    }


    private void UpdateFacingPoints()
    {
        // Move Tackle hit point to whichever side Eevee faces.
        if (attackPoint != null)
        {
            Vector3 attackPosition = attackPoint.localPosition;

            attackPosition.x =
                Mathf.Abs(attackPosition.x) *
                (facingRight ? 1 : -1);

            attackPoint.localPosition = attackPosition;
        }

        // Move Swift spawn point to whichever side Eevee faces.
        if (projectilePoint != null)
        {
            Vector3 projectilePosition = projectilePoint.localPosition;

            projectilePosition.x =
                Mathf.Abs(projectilePosition.x) *
                (facingRight ? 1 : -1);

            projectilePoint.localPosition = projectilePosition;
        }
    }


    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            horizontal * moveSpeed,
            rb.linearVelocity.y
        );
    }


    private void OnDrawGizmosSelected()
    {
        // Ground detection circle
        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(
                groundCheck.position,
                groundCheckRadius
            );
        }

        // Tackle range
        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(
                attackPoint.position,
                attackRange
            );
        }
    }
}