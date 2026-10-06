using UnityEngine;
using TMPro;

public class HighScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;

    private void Start()
    {
        RefreshHighScore();
    }

    public void RefreshHighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (highScoreText != null)
        {
            highScoreText.text = "HIGH SCORE: " + highScore;
        }
    }

    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("HighScore", 0);
        PlayerPrefs.Save();

        RefreshHighScore();

        Debug.Log("High Score Reset");
    }
}