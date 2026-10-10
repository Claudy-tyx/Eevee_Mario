using UnityEngine;

[CreateAssetMenu(
    fileName = "EeveeSpeedBoostAction",
    menuName = "Eevee Game/FSM/Buffs/Speed Boost Action"
)]
public class EeveeSpeedBoostAction : FSMAction
{
    public override void Act(StateController controller)
    {
        EeveeBuffFSMController buff =
            controller as EeveeBuffFSMController;

        if (buff == null)
            return;

        // The controller manages the buff timer and multiplier.
        // We don't reactivate the buff here because that would
        // reset its timer every frame.
    }
}