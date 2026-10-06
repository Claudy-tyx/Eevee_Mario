using UnityEngine;

[CreateAssetMenu(
    fileName = "RunData",
    menuName = "Eevee Game/Run Data"
)]
public class RunData : ScriptableObject
{
    [Header("Runtime Run Data")]
    [SerializeField] private int currentScore = 0;
    [SerializeField] private bool runActive = false;

    public int CurrentScore => currentScore;
    public bool RunActive => runActive;

    public void StartRun()
    {
        currentScore = 0;
        runActive = true;
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
    }

    public void ResetRun()
    {
        currentScore = 0;
        runActive = false;
    }
}