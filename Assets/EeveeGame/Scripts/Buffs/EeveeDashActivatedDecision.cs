using UnityEngine;

[CreateAssetMenu(
    fileName = "EeveeDashActivatedDecision",
    menuName = "Eevee Game/FSM/Buffs/Dash Activated Decision"
)]
public class EeveeDashActivatedDecision : FSMDecision
{
    public override bool Decide(StateController controller)
    {
        EeveeBuffFSMController buff =
            controller as EeveeBuffFSMController;

        if (buff == null)
            return false;

        return buff.IsDashRequested;
    }
}