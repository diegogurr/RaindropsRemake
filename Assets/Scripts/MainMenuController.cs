using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject tutorialMenu;
    [SerializeField] GameObject mainMenu;

    public void LoadScene(string sceneToLoad) 
    {
        SceneManager.LoadScene(sceneToLoad);
        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowTutorial(bool show) 
    {
        tutorialMenu.SetActive(show);
    }

    public void ShowMainMenu(bool show)
    {
        mainMenu.SetActive(show);
    }

    public void SetGameMode(int mode)
    {
        // 0 for Normal, 1 for Binary, 2 for Nightmare
        PlayerPrefs.SetInt("GameMode", mode); 
        PlayerPrefs.Save();
    }

}
