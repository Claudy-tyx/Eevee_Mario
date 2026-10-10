using System;
using UnityEngine;

[Serializable]
public class FSMTransition
{
    public FSMDecision decision;

    public State trueState;
    public State falseState;
}