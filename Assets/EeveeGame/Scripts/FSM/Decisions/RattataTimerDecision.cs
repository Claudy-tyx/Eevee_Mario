using UnityEngine;

[CreateAssetMenu(
    fileName = "RattataTimerDecision",
    menuName = "Eevee Game/FSM/Decisions/Rattata Timer Finished"
)]
public class RattataTimerDecision : FSMDecision
{
    public override bool Decide(StateController controller)
    {
        RattataFSMController rattata =
            controller as RattataFSMController;

        return rattata != null && rattata.IsTimerFinished;
    }
}