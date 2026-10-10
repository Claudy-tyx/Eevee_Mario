using UnityEngine;

[CreateAssetMenu(
    fileName = "RattataIdleAction",
    menuName = "Eevee Game/FSM/Actions/Rattata Idle"
)]
public class RattataIdleAction : FSMAction
{
    public override void Act(StateController controller)
    {
        Rigidbody2D rb =
            controller.GetComponent<Rigidbody2D>();

        Animator animator =
            controller.GetComponent<Animator>();

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );
        }

        if (animator != null)
        {
            animator.SetBool("Walking", false);
        }
    }
}