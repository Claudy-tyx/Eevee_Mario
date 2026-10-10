using UnityEngine;

[CreateAssetMenu(
    fileName = "RattataPlayerDetectedDecision",
    menuName = "Eevee Game/FSM/Decisions/Player Detected"
)]
public class RattataPlayerDetectedDecision : FSMDecision
{
    public override bool Decide(StateController controller)
    {
        RattataFSMController rattata =
            controller as RattataFSMController;

        if (rattata == null)
            return false;

        bool detected = rattata.IsPlayerDetected();

        if (detected)
        {
            Debug.Log("Rattata detected Eevee!");
        }

        return detected;
    }
}