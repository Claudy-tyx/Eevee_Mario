using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [Header("Loading UI")]
    [SerializeField] private Image progressBarFill;
    [SerializeField] private TMP_Text loadingText;

    [Header("Loading Settings")]
    [SerializeField] private string tutorialSceneName = "Tutorial";
    [SerializeField] private float minimumLoadTime = 3f;

    private AsyncOperation loadingOperation;
    private bool readyToContinue = false;

    private void Start()
    {
        progressBarFill.fillAmount = 0f;
        StartCoroutine(LoadTutorial());
    }

    private IEnumerator LoadTutorial()
    {
        loadingOperation = SceneManager.LoadSceneAsync(tutorialSceneName);
        loadingOperation.allowSceneActivation = false;

        float timer = 0f;

        while (timer < minimumLoadTime ||
               loadingOperation.progress < 0.9f)
        {
            timer += Time.unscaledDeltaTime;

            // Progress caused by our minimum display time
            float timeProgress =
                Mathf.Clamp01(timer / minimumLoadTime);

            // Actual Unity loading progress
            float sceneProgress =
                Mathf.Clamp01(loadingOperation.progress / 0.9f);

            // Don't show 100% until both are finished
            float progress = Mathf.Min(timeProgress, sceneProgress);

            progressBarFill.fillAmount = progress;

            if (loadingText != null)
            {
                loadingText.text =
                    "Loading... " +
                    Mathf.RoundToInt(progress * 100f) + "%";
            }

            yield return null;
        }

        progressBarFill.fillAmount = 1f;

        if (loadingText != null)
            loadingText.text = "Press anywhere to continue";

        readyToContinue = true;
    }

    private void Update()
    {
        if (!readyToContinue)
            return;

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            loadingOperation.allowSceneActivation = true;
        }

        if (Keyboard.current != null &&
            Keyboard.current.anyKey.wasPressedThisFrame)
        {
            loadingOperation.allowSceneActivation = true;
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("StartScreen");
    }
}