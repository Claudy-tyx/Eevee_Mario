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
    [SerializeField] private LayerMask breakableLayer;

    [Header("Swift")]
    [SerializeField] private Transform projectilePoint;
    [SerializeField] private GameObject swiftProjectilePrefab;
    [SerializeField] private float swiftCooldown = 0.4f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem flowerParticles;
    [SerializeField] private ParticleSystem landingParticles;

    [Header("Sound Effects")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip tackleSound;
    [SerializeField] private AudioClip swiftSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip faintSound;
    [SerializeField] private AudioClip landingSound;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.18f;
    [SerializeField] private float dashCooldown = 2f;

    [SerializeField] private PlayerStats playerStats;

    

    private Rigidbody2D rb;

    private float horizontal;
    private bool isGrounded;
    private bool hasCheckedGround;
    private bool wasGrounded;
    private bool facingRight = true;

    private float nextAttackTime;
    private float nextSwiftTime;

    private bool isDashing;
    private float dashTimer;
    private float nextDashTime;
    private float dashDirection;

    private int airJumpsRemaining;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (playerStats == null)
            playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        CheckGround();
        HandleMovementInput();
        HandleJump();
        HandleAttacks();
        UpdateAnimations();
        UpdateFacingPoints();
        HandleDash();
    }

    private void CheckGround()
    {
        if (groundCheck == null)
            return;

        wasGrounded = isGrounded;

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        // Don't count spawning on the ground as a landing.
        if (!hasCheckedGround)
        {
            hasCheckedGround = true;
            wasGrounded = isGrounded;

            if (isGrounded && playerStats != null)
            {
                airJumpsRemaining =
                    playerStats.GetExtraJumps();
            }

            return;
        }

        // Eevee changed from airborne → grounded.
        if (!wasGrounded && isGrounded)
        {
            Land();
        }
    }

    private void HandleDash()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.leftShiftKey.wasPressedThisFrame &&
            Time.time >= nextDashTime &&
            !isDashing)
        {
            StartDash();
        }
    }

    private void StartDash()
    {
        isDashing = true;

        dashTimer = dashDuration;
        float currentDashCooldown = dashCooldown;

        if (playerStats != null)
        {
            currentDashCooldown *=
                playerStats.GetDashCooldownMultiplier();
        }

        nextDashTime =
            Time.time + currentDashCooldown;

        dashDirection = facingRight ? 1f : -1f;
    }

    private void Land()
    {
        if (landingParticles != null)
        {
            landingParticles.Play();
        }

        if (playerStats != null)
        {
            airJumpsRemaining =
                playerStats.GetExtraJumps();
        }

        PlaySound(landingSound);
    }

    private void HandleMovementInput()
    {
        horizontal = 0f;

        if (Keyboard.current == null)
            return;

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
        if (Keyboard.current == null || isDashing)
            return;

        if (!Keyboard.current.spaceKey.wasPressedThisFrame)
            return;

        // Normal ground jump.
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            PlayJumpSound();
            return;
        }

        // Extra mid-air jump.
        if (airJumpsRemaining > 0)
        {
            airJumpsRemaining--;

            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            // Make the extra jump visually obvious.
            ExtraJumpEffect();

            PlayJumpSound();
        }
    }

    private void PlayJumpSound()
    {
        // 30% chance for Eevee to make a sound.
        if (Random.value < 0.3f)
        {
            PlaySound(jumpSound);
        }
    }

    public void PlayFlowerParticles()
    {
        if (flowerParticles != null)
            flowerParticles.Play();
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
            int damage = 1;
            float speedMultiplier = 1f;
            float lifetimeMultiplier = 1f;
            bool homingEnabled = false;

            Vector2 direction =
                facingRight
                ? Vector2.right
                : Vector2.left;

            if (playerStats != null)
            {
                damage =
                    playerStats.GetSwiftDamage();

                homingEnabled =
                    playerStats.HasHomingSwift();

                speedMultiplier =
                    playerStats.GetSwiftSpeedMultiplier();

                lifetimeMultiplier =
                    playerStats.GetSwiftLifetimeMultiplier();

                // Directional Swift only controls aim when
                if (playerStats.HasDirectionalSwift() &&
                    !homingEnabled)
                {
                    direction =
                        GetSwiftAimDirection();
                }
            }

            swift.Setup(
                direction,
                damage,
                speedMultiplier,
                lifetimeMultiplier,
                homingEnabled
            );
        }

        // Swift sound happens when projectile actually appears.
        PlaySound(swiftSound);
    }

    private void ExtraJumpEffect()
    {
        // Restart the jump animation.
        if (animator != null)
        {
            animator.Play("Eevee_Hop", 0, 0f);
        }

        // Reuse the landing particle burst at Eevee's feet.
        if (landingParticles != null)
        {
            landingParticles.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );

            landingParticles.Play();
        }

        // Reuse landing thud sound.
        PlaySound(landingSound);
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

    private Vector2 GetSwiftAimDirection()
    {
        if (Mouse.current == null ||
            Camera.main == null ||
            projectilePoint == null)
        {
            return facingRight
                ? Vector2.right
                : Vector2.left;
        }

        Vector2 mouseScreenPosition =
            Mouse.current.position.ReadValue();

        Vector3 mouseWorldPosition =
            Camera.main.ScreenToWorldPoint(
                new Vector3(
                    mouseScreenPosition.x,
                    mouseScreenPosition.y,
                    Mathf.Abs(
                        Camera.main.transform.position.z
                    )
                )
            );

        Vector2 direction =
            (Vector2)mouseWorldPosition -
            (Vector2)projectilePoint.position;

        if (direction.sqrMagnitude <= 0.001f)
        {
            return facingRight
                ? Vector2.right
                : Vector2.left;
        }

        // Prevent Swift from firing behind Eevee.
        if (facingRight)
        {
            direction.x =
                Mathf.Max(direction.x, 0.05f);
        }
        else
        {
            direction.x =
                Mathf.Min(direction.x, -0.05f);
        }

        return direction.normalized;
    }

    private void Attack()
    {
        nextAttackTime = Time.time + attackCooldown;

        if (animator != null)
            animator.SetTrigger("Attack");

        PlaySound(tackleSound);

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            breakableLayer
        );

        foreach (Collider2D hit in hits)
        {
            BreakableBox box =
                hit.GetComponentInParent<BreakableBox>();

            if (box != null)
                box.Hit();
        }
    }

    private void ShootSwift()
    {
        nextSwiftTime = Time.time + swiftCooldown;

        if (animator != null)
            animator.SetTrigger("Shoot");
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Called by PlayerHealth when Eevee takes damage.
    public void PlayHurtSound()
    {
        PlaySound(hurtSound);
    }

    // Called by PlayerHealth when Eevee reaches 0 HP.
    public void PlayFaintSound()
    {
        PlaySound(faintSound);
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
        if (attackPoint != null)
        {
            Vector3 attackPosition = attackPoint.localPosition;

            attackPosition.x =
                Mathf.Abs(attackPosition.x) *
                (facingRight ? 1 : -1);

            attackPoint.localPosition = attackPosition;
        }

        if (projectilePoint != null)
        {
            Vector3 projectilePosition =
                projectilePoint.localPosition;

            projectilePosition.x =
                Mathf.Abs(projectilePosition.x) *
                (facingRight ? 1 : -1);

            projectilePoint.localPosition =
                projectilePosition;
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = new Vector2(
                dashDirection * dashSpeed,
                rb.linearVelocity.y
            );

            dashTimer -= Time.fixedDeltaTime;

            if (dashTimer <= 0f)
            {
                isDashing = false;
            }

            return;
        }

        float currentMoveSpeed = moveSpeed;

        if (playerStats != null)
        {
            currentMoveSpeed *=
                playerStats.GetMoveSpeedMultiplier();
        }

        rb.linearVelocity = new Vector2(
            horizontal * currentMoveSpeed,
            rb.linearVelocity.y
        );
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(
                groundCheck.position,
                groundCheckRadius
            );
        }

        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(
                attackPoint.position,
                attackRange
            );
        }
    }
}