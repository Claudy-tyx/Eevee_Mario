using UnityEngine;

[CreateAssetMenu(
    fileName = "EeveeBuffExpiredDecision",
    menuName = "Eevee Game/FSM/Buffs/Buff Expired Decision"
)]
public class EeveeBuffExpiredDecision : FSMDecision
{
    public override bool Decide(StateController controller)
    {
        EeveeBuffFSMController buff =
            controller as EeveeBuffFSMController;

        if (buff == null)
            return false;

        return buff.IsBuffExpired;
    }
}