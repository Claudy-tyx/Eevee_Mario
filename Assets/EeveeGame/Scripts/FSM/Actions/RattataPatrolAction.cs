using UnityEngine;

[CreateAssetMenu(
    fileName = "RattataPatrolAction",
    menuName = "Eevee Game/FSM/Actions/Rattata Patrol"
)]
public class RattataPatrolAction : FSMAction
{
    [SerializeField] private float moveSpeed = 1.5f;

    public override void Act(StateController controller)
    {
        RattataFSMController rattata =
            controller as RattataFSMController;

        if (rattata == null)
        {
            Debug.LogError("Rattata FSM Controller is NULL!");
            return;
        }

        Rigidbody2D rb = rattata.GetComponent<Rigidbody2D>();
        SpriteRenderer sprite = rattata.GetComponentInChildren<SpriteRenderer>();
        Animator animator = controller.GetComponentInChildren<Animator>();

        if (rb == null || sprite == null)
        {
            Debug.LogError(
                $"Missing components! Rigidbody2D: {rb != null}, SpriteRenderer: {sprite != null}"
            );
            return;
        }

        int direction = sprite.flipX ? -1 : 1;

        if (!rattata.HasGroundAhead(direction))
        {
            direction *= -1;
            sprite.flipX = direction < 0;
        }

        rb.linearVelocity = new Vector2(
            direction * moveSpeed,
            rb.linearVelocity.y
        );

        Debug.Log($"Patrol movement applied: {rb.linearVelocity.x}, Speed: {moveSpeed}");

        if (animator != null)
            animator.SetBool("Walking", true);
    }
}