using UnityEngine;

[CreateAssetMenu(
    fileName = "NewState",
    menuName = "Eevee Game/FSM/State"
)]
public class State : ScriptableObject
{
    [Header("Actions")]
    [SerializeField] private FSMAction[] actions;

    [Header("Transitions")]
    [SerializeField] private FSMTransition[] transitions;

    public void UpdateState(StateController controller)
    {
        // Execute all actions belonging to this state.
        if (actions != null)
        {
            foreach (FSMAction action in actions)
            {
                if (action != null)
                    action.Act(controller);
            }
        }

        // Evaluate whether we should change states.
        if (transitions != null)
        {
            foreach (FSMTransition transition in transitions)
            {
                if (transition == null ||
                    transition.decision == null)
                    continue;

                bool result =
                    transition.decision.Decide(controller);

                State nextState = result
                    ? transition.trueState
                    : transition.falseState;

                if (nextState != null)
                {
                    controller.TransitionToState(nextState);
                    break;
                }
            }
        }
    }
}