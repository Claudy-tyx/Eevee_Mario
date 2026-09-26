using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string tutorialSceneName = "Tutorial";

    [Header("Input Delay")]
    [SerializeField] private float inputDelay = 0.25f;

    private float timer;
    private bool starting = false;


    private void Start()
    {
        // Make sure the game isn't still paused
        // from a previous completion/game over screen.
        Time.timeScale = 1f;

        timer = inputDelay;
    }


    private void Update()
    {
        if (starting)
            return;

        if (timer > 0f)
        {
            timer -= Time.unscaledDeltaTime;
            return;
        }

        bool startPressed = false;


        // Any keyboard key.
        if (Keyboard.current != null &&
            Keyboard.current.anyKey.wasPressedThisFrame)
        {
            startPressed = true;
        }


        // Mouse click.
        if (Mouse.current != null &&
            (Mouse.current.leftButton.wasPressedThisFrame ||
             Mouse.current.rightButton.wasPressedThisFrame))
        {
            startPressed = true;
        }


        if (startPressed)
        {
            StartGame();
        }
    }


    private void StartGame()
    {
        if (starting)
            return;

        starting = true;

        // Starting from the title screen means
        // this is a completely new run.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetRun();
        }

        SceneManager.LoadScene(tutorialSceneName);
    }
}