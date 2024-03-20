using UnityEngine;

public class HighScoreController : MonoBehaviour
{
    private int[] highScoresNormal = new int[5];
    private int[] highScoresBinary = new int[5];
    private int[] highScoresNightmare = new int[5];

    private void Awake()
    {
        LoadHighScores(GameController.GameMode.Normal, ref highScoresNormal);
        LoadHighScores(GameController.GameMode.Binary, ref highScoresBinary);
        LoadHighScores(GameController.GameMode.Nightmare, ref highScoresNightmare);
    }

    public void TryAddHighScore(int newScore, GameController.GameMode gameMode)
    {
        int[] highScores = GetHighScoresArray(gameMode);
        for (int i = 0; i < highScores.Length; i++)
        {
            if (newScore > highScores[i])
            {
                for (int j = highScores.Length - 1; j > i; j--)
                {
                    highScores[j] = highScores[j - 1];
                }
                highScores[i] = newScore;
                SaveHighScores(gameMode, highScores);
                break;
            }
        }
    }

    private void LoadHighScores(GameController.GameMode gameMode, ref int[] highScores)
    {
        string prefix = GetModePrefix(gameMode);
        for (int i = 0; i < highScores.Length; i++)
        {
            highScores[i] = PlayerPrefs.GetInt($"{prefix}HighScore{i}", 0);
        }
    }

    private void SaveHighScores(GameController.GameMode gameMode, int[] highScores)
    {
        string prefix = GetModePrefix(gameMode);
        for (int i = 0; i < highScores.Length; i++)
        {
            PlayerPrefs.SetInt($"{prefix}HighScore{i}", highScores[i]);
        }
        PlayerPrefs.Save();
    }

    private int[] GetHighScoresArray(GameController.GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameController.GameMode.Normal:
                return highScoresNormal;
            case GameController.GameMode.Binary:
                return highScoresBinary;
            case GameController.GameMode.Nightmare:
                return highScoresNightmare;
            default:
                return null;
        }
    }

    private string GetModePrefix(GameController.GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameController.GameMode.Normal:
                return "Normal_";
            case GameController.GameMode.Binary:
                return "Binary_";
            case GameController.GameMode.Nightmare:
                return "Nightmare_";
            default:
                return ""; 
        }
    }

    public int[] GetHighScores(GameController.GameMode gameMode)
    {
        return GetHighScoresArray(gameMode);
    }
}
