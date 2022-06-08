using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen instance;

    [Header("Loading to Main Menu")]
    public string mainMenuScene;

    [Header("Game Over Menu Variables")]
    public float waitToShowGameOver;
    [HideInInspector] public bool isGameOver;
    public GameObject gameOverScreen, restartButton;
    public GameObject[] buttons;
    public CanvasGroup gameOverMenu, creditsMenu;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ChangeActiveButtons(int buttonToChoose)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(buttons[buttonToChoose]);
    }

    public void ShowGameOver()
    {
        StartCoroutine(ShowGameOverCo());
    }

    public IEnumerator ShowGameOverCo()
    {
        yield return new WaitForSeconds(waitToShowGameOver);

        //Display the game over screen and set some bools so player can not pause the game
        gameOverScreen.SetActive(true);
        isGameOver = true;
        PauseMenu.instance.canPause = false;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(restartButton);


        //Open the game over menu by calling the function and setting the timescale to 0
        OpenGameOverMenu();
        Time.timeScale = 0;
    }

    public void RestartLevel()
    {
        ChangeTimeScale();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Function for opening the game over menu by setting alpha to 1
    public void OpenGameOverMenu()
    {
        gameOverMenu.alpha = 1;
        gameOverMenu.blocksRaycasts = true;
    }

    //Function for closing the game over menu by setting alpha to 0
    public void CloseGameOverMenu()
    {
        gameOverMenu.alpha = 0;
        gameOverMenu.blocksRaycasts = false;
    }

    public void OpenCredits()
    {
        creditsMenu.alpha = 1;
        creditsMenu.blocksRaycasts = true;
    }

    public void CloseCredits()
    {
        creditsMenu.alpha = 0;
        creditsMenu.blocksRaycasts = false;
    }

    public void LoadMainMenu()
    {
        ChangeTimeScale();
        SceneManager.LoadScene(mainMenuScene);
    }

    public void ChangeTimeScale()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    public void PlayButtonSound()
    {
        AudioManager.instance.PlaySFX(4);
    }
}
