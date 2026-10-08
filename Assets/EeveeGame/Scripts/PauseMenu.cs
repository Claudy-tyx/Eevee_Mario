using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private string startSceneName = "StartScreen";

    private bool isPaused;

    private void Awake()
    {
        isPaused = false;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        if (isPaused)
            return;

        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        AudioListener.pause = true;
    }

    public void Resume()
    {
        if (!isPaused)
            return;

        isPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void Restart()
    {
        isPaused = false;

        Time.timeScale = 1f;
        AudioListener.pause = false;

        if (GameManager.Instance != null)
            GameManager.Instance.ResetRun();

        SceneManager.LoadScene(startSceneName);
    }

    private void OnDisable()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
            isPaused = false;
        }
    }
}