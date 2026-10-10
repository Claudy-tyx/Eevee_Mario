using UnityEngine;

public abstract class FSMAction : ScriptableObject
{
    public abstract void Act(StateController controller);
}