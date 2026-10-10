using UnityEngine;

[CreateAssetMenu(
    fileName = "EeveeNormalBuffAction",
    menuName = "Eevee Game/FSM/Buffs/Normal Action"
)]
public class EeveeNormalBuffAction : FSMAction
{
    public override void Act(StateController controller)
    {
        EeveeBuffFSMController buff =
            controller as EeveeBuffFSMController;

        if (buff == null)
            return;

        buff.DeactivateSpeedBuff();
    }
}