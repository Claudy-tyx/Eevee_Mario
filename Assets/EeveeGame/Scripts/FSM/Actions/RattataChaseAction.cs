using UnityEngine;

[CreateAssetMenu(
    fileName = "RattataChaseAction",
    menuName = "Eevee Game/FSM/Actions/Rattata Chase"
)]
public class RattataChaseAction : FSMAction
{
    [Header("Chase Movement")]
    [SerializeField] private float chaseSpeed = 2.5f;

    public override void Act(StateController controller)
    {
        RattataFSMController rattata =
            controller as RattataFSMController;

        if (rattata == null || rattata.Player == null)
            return;

        Rigidbody2D rb = rattata.GetComponent<Rigidbody2D>();
        SpriteRenderer sprite =
            rattata.GetComponentInChildren<SpriteRenderer>();
        Animator animator =
            rattata.GetComponentInChildren<Animator>();

        if (rb == null || sprite == null)
            return;

        // Determine which direction Eevee is in.
        float difference =
            rattata.Player.position.x - rattata.transform.position.x;

        int direction = difference >= 0f ? 1 : -1;

        // Face Eevee.
        sprite.flipX = direction < 0;

        // Don't chase Eevee off platform edges.
        if (!rattata.HasGroundAhead(direction))
        {
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );

            if (animator != null)
                animator.SetBool("Walking", false);

            return;
        }

        // Chase Eevee horizontally.
        rb.linearVelocity = new Vector2(
            direction * chaseSpeed,
            rb.linearVelocity.y
        );

        if (animator != null)
            animator.SetBool("Walking", true);
    }
}