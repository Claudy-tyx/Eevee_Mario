using UnityEngine;

public class EeveeBuffFSMController : StateController
{
    [Header("Speed Buff")]
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private float buffDuration = 3f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buffStartSound;
    [SerializeField] private AudioClip buffEndSound;

    private float buffTimer;
    private bool buffActive;

    public float SpeedMultiplier =>
        buffActive ? speedMultiplier : 1f;

    public bool IsBuffExpired =>
        buffActive && buffTimer <= 0f;

    private bool dashRequested;

    public bool IsDashRequested => dashRequested;

    public void RequestDashBuff()
    {
        dashRequested = true;
    }

    protected override void OnStateEntered(State state)
    {
        if (state.name == "Eevee_SpeedBoostState")
        {
            ActivateSpeedBuff();
        }
        else if (state.name == "Eevee_NormalState")
        {
            DeactivateSpeedBuff();
        }
    }

    protected override void Update()
    {
        if (buffActive)
        {
            buffTimer -= Time.deltaTime;
        }

        base.Update();
    }

    public void ActivateSpeedBuff()
    {
        dashRequested = false;
        buffActive = true;
        buffTimer = buffDuration;

        if (audioSource != null && buffStartSound != null)
        {
            audioSource.PlayOneShot(buffStartSound);
        }
    }

    public void DeactivateSpeedBuff()
    {
        if (!buffActive)
            return;

        buffActive = false;
        buffTimer = 0f;

        if (audioSource != null && buffEndSound != null)
        {
            audioSource.PlayOneShot(buffEndSound);
        }
    }

    private void OnDisable()
    {
        buffActive = false;
        buffTimer = 0f;
    }
}