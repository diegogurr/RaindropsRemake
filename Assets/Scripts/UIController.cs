using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;

    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TextMeshProUGUI gameOverCorrectText;
    [SerializeField] private TextMeshProUGUI gameOverAccuracyText;

    [SerializeField] private TextMeshProUGUI highScoresText;

    [SerializeField] InputController inputController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    public void UpdateLives(int lives)
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + lives.ToString();
        }
    }

    public void ShowGameOver(bool show)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(show);
        }
    }
    public void ShowPauseMenu(bool show)
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(show);
        }
        if (pausePanel.activeSelf)
        {
            inputController.DisableInput();
            Time.timeScale = 0.0f;
        }
        else
        {
            inputController.EnableInput();
            Time.timeScale = 1.0f;
        }
    }
    public void UpdateGameOverValues(int score, int correctDrops, int missedDrops, int[] highScores)
    {
        gameOverScoreText.text = "Score: " + score.ToString();
        gameOverCorrectText.text = "Correct: " + correctDrops.ToString();

        int accuracy = (correctDrops + missedDrops) > 0 ? (correctDrops * 100) / (correctDrops + missedDrops) : 0;
        gameOverAccuracyText.text = "Accuracy: " + accuracy.ToString() + "%";

        string highScoresString = $"High Scores ({GameController.Instance.GetCurrentGameMode()}):\n";
        for (int i = 0; i < highScores.Length; i++)
        {
            highScoresString += (i + 1).ToString() + ". " + highScores[i].ToString() + "\n";
        }
        highScoresText.text = highScoresString;
    }

}
