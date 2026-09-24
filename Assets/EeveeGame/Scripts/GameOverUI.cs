using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup gameOverPanel;

    [Header("Fade")]
    [SerializeField] private float fadeDuration = 1f;

    [SerializeField] private TMP_Text enemiesDefeatedText;

    private int enemiesDefeated = 0;


    private void Start()
    {
        gameOverPanel.alpha = 0f;
        gameOverPanel.interactable = false;
        gameOverPanel.blocksRaycasts = false;
    }


    public void ShowGameOver()
    {
        if (enemiesDefeatedText != null)
        {
            enemiesDefeatedText.text =
                "Enemies Defeated: " + enemiesDefeated;
        }

        StartCoroutine(FadeIn());
    }

    public void AddEnemyDefeated()
    {
        enemiesDefeated++;
    }


    private IEnumerator FadeIn()
    {
        float timer = 0f;

        gameOverPanel.blocksRaycasts = true;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;

            gameOverPanel.alpha =
                Mathf.Clamp01(timer / fadeDuration);

            yield return null;
        }

        gameOverPanel.alpha = 1f;
        gameOverPanel.interactable = true;
    }


    public void RestartGame()
    {
        // Unfreeze game first.
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}