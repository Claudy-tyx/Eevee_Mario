using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompletionUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject completionPanel;

    [Header("Stats")]
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text powerUpsText;
    [SerializeField] private TMP_Text deathsText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text scoreText;

    [Header("New Game")]
    [SerializeField] private string startSceneName = "Tutorial";


    private void Start()
    {
        if (completionPanel != null)
        {
            completionPanel.SetActive(false);
        }
    }


    // =========================
    // SHOW RESULTS
    // =========================

    public void ShowCompletion()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning(
                "CompletionUI: GameManager not found."
            );

            return;
        }

        int kills =
            GameManager.Instance.GetTotalKills();

        int powerUps =
            GameManager.Instance.GetCrystalsCollected();

        int deaths =
            GameManager.Instance.GetDeathCount();

        float runTime =
            GameManager.Instance.GetRunTime();


        // Calculate score.
        int finalScore = CalculateScore(
            kills,
            powerUps,
            deaths,
            runTime
        );

        // Save as high score if this completed run
        // beats the previous best score.
        GameManager.Instance.TrySetHighScore(finalScore);


        // Update UI.
        if (killsText != null)
        {
            killsText.text =
                "Enemies Defeated: " + kills;
        }

        if (powerUpsText != null)
        {
            powerUpsText.text =
                "Power-Ups Collected: " + powerUps;
        }

        if (deathsText != null)
        {
            deathsText.text =
                "Deaths: " + deaths;
        }

        if (timeText != null)
        {
            timeText.text =
                "Time: " + FormatTime(runTime);
        }

        if (scoreText != null)
        {
            scoreText.text =
                "Final Score: " + finalScore;
        }


        completionPanel.SetActive(true);

        // Ending screen stays paused.
        Time.timeScale = 0f;
    }


    // =========================
    // TIME
    // =========================

    private string FormatTime(float time)
    {
        int totalSeconds =
            Mathf.FloorToInt(time);

        int minutes =
            totalSeconds / 60;

        int seconds =
            totalSeconds % 60;

        return string.Format(
            "{0:00}:{1:00}",
            minutes,
            seconds
        );
    }


    // =========================
    // SCORE
    // =========================

    private int CalculateScore(
        int kills,
        int powerUps,
        int deaths,
        float runTime
    )
    {
        int score = 10000;

        // Reward enemies defeated.
        score += kills * 100;

        // Reward collecting upgrades.
        score += powerUps * 500;

        // Penalise deaths.
        score -= deaths * 500;

        // Penalise time:
        // 10 points per second.
        score -= Mathf.FloorToInt(runTime) * 10;

        // Never display a negative score.
        return Mathf.Max(score, 0);
    }


    // =========================
    // PLAY AGAIN
    // =========================

    public void PlayAgain()
    {
        Time.timeScale = 1f;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetRun();
        }

        SceneManager.LoadScene(startSceneName);
    }
}