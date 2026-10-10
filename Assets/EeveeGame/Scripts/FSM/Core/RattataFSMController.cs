using UnityEngine;

public class RattataFSMController : StateController
{
    [Header("Idle Duration")]
    [SerializeField] private float minIdleTime = 0.5f;
    [SerializeField] private float maxIdleTime = 2f;

    [Header("Patrol Duration")]
    [SerializeField] private float minPatrolTime = 1.5f;
    [SerializeField] private float maxPatrolTime = 4f;

    [Header("Ground Detection")]
    [SerializeField] private Transform edgeCheck;
    [SerializeField] private float edgeForwardDistance = 0.4f;
    [SerializeField] private float edgeDownDistance = 0.6f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Player Detection")]
    [SerializeField] private float detectionRange = 4f;

    private Transform player;

    public Transform Player => player;

    public bool HasGroundAhead(int direction)
    {
        if (edgeCheck == null)
            return true;

        Vector2 rayStart = new Vector2(
            edgeCheck.position.x + edgeForwardDistance * direction,
            edgeCheck.position.y
        );

        bool groundAhead = Physics2D.Raycast(
            rayStart,
            Vector2.down,
            edgeDownDistance,
            groundLayer
        );

        Debug.DrawRay(
            rayStart,
            Vector2.down * edgeDownDistance,
            groundAhead ? Color.green : Color.red
        );

        return groundAhead;
    }

    private float stateTimer;

    public bool IsTimerFinished => stateTimer <= 0f;

    protected override void Update()
    {
        stateTimer -= Time.deltaTime;

        base.Update();
    }

    public bool IsPlayerDetected()
    {
        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
                player = playerObject.transform;
        }

        if (player == null)
            return false;

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        return distance <= detectionRange;
    }

    public void StartIdleTimer()
    {
        stateTimer = Random.Range(minIdleTime, maxIdleTime);
    }

    public void StartPatrolTimer()
    {
        stateTimer = Random.Range(minPatrolTime, maxPatrolTime);
    }

    protected override void OnStateEntered(State state)
    {
        if (state.name == "Rattata_IdleState")
        {
            StartIdleTimer();
        }
        else if (state.name == "Rattata_PatrolState")
        {
            StartPatrolTimer();
        }
    }
}