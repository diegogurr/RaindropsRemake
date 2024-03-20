using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GameMode
    {
        Normal,
        Binary,
        Nightmare
    }
    public static GameController Instance { get; private set; }

    public DropController dropController;
    public HighScoreController highScoreController;
    public InputController inputController;

    public GameObject waterLevel;
    private Vector3 waterLevelStartPosition;

    [SerializeField] private GameMode currentGameMode = GameMode.Normal;

    public bool canSpawnPowerUps = true;

    private int score = 0;
    public int lives = 3;
    private int solvedDrops = 0;
    private int missedDrops = 0;

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
        waterLevelStartPosition=waterLevel.transform.position;
        canSpawnPowerUps = true;
    }

    void Start()
    {
        SetDifficulty();
        StartGame();
    }

    void SetDifficulty() 
    {
            int mode = PlayerPrefs.GetInt("GameMode", 0);
            switch (mode)
            {
                case 0:
                    currentGameMode = GameMode.Normal;
                    break;
                case 1:
                    currentGameMode = GameMode.Binary;
                    break;
                case 2:
                    currentGameMode = GameMode.Nightmare;
                    break;
            }
        print(currentGameMode);
    }

    public GameMode GetCurrentGameMode() 
    {
        return currentGameMode;
    }

    public void StartGame()
    {
        ResetGame();
        inputController.EnableInput();
        UIController.Instance.ShowGameOver(false);
        UIController.Instance.UpdateScore(score);
        dropController.StartGeneratingDrops();
    }

    public void IncreaseDifficulty()
    {
        dropController.IncreaseDifficultyLevel();
    }

    public void ResolveDrop(int points)
    {
        score += points;
        solvedDrops++;
        AudioController.Instance.PlaySound("DropSolved");
        UIController.Instance.UpdateScore(score);
    }

    public void LoseLife()
    {
        lives--;
        missedDrops++;
        dropController.StopGeneratingDrops();
        dropController.ExplodeAllDrops();
        canSpawnPowerUps = false;

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(WaitAndContinue(1.5f));
        }
    }

    IEnumerator WaitAndContinue(float waitTime)
    {
        IncreaseWaterLevel();
        yield return new WaitForSeconds(waitTime);
        canSpawnPowerUps = true;
        dropController.StartGeneratingDrops();
    }

    void ResetGame()
    {
        canSpawnPowerUps = true;
        UIController.Instance.ShowGameOver(false);
        ResetWaterLevel();
        solvedDrops=0;
        missedDrops=0;
        score=0;
        lives=3;
    }
    void IncreaseWaterLevel()
    {
        AudioController.Instance.PlaySound("WaterSplash");
        waterLevel.transform.position += new Vector3(0, 0.5f, 0);
    }
    void ResetWaterLevel() 
    {
        waterLevel.transform.position = waterLevelStartPosition;
    }

    void GameOver()
    {
        AudioController.Instance.PlaySound("WaterSplash");
        highScoreController.TryAddHighScore(score, currentGameMode);
        inputController.DisableInput();
        int[] highScores = highScoreController.GetHighScores(currentGameMode);
        UIController.Instance.UpdateGameOverValues(score, solvedDrops, missedDrops, highScores);
        UIController.Instance.ShowGameOver(true);
        canSpawnPowerUps = false;
    }
}
