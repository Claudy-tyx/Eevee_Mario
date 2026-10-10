using UnityEngine;

public class StateController : MonoBehaviour
{
    [Header("FSM")]
    [SerializeField] private State initialState;

    private State currentState;

    public State CurrentState => currentState;

    private void Start()
    {
        if (initialState != null)
        {
            TransitionToState(initialState);
        }
    }

    protected virtual void Update()
    {
        if (currentState == null)
            return;

        currentState.UpdateState(this);
    }

    public void TransitionToState(State nextState)
    {
        if (nextState == null || nextState == currentState)
            return;

        currentState = nextState;

        OnStateEntered(currentState);

        Debug.Log(
            gameObject.name + " entered state: " + currentState.name
        );
    }

    protected virtual void OnStateEntered(State state)
    {
        // Child FSM controllers can override this method.
    }
}