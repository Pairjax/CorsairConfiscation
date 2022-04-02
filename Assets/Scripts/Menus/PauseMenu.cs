using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    [Header("Pausing Variables")]
    public bool isPaused;
    public GameObject pauseScreen;
    public GameObject resumeButton;
    public bool canPause = true;

    [Header("Loadingt to Main Menu")]
    public string mainMenuScene;

    [Header("Pause Menu Variables")]
    public GameObject[] buttons;
    public CanvasGroup controlsMenu, optionsMenu, creditsMenu;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetButtonDown("Pause") && canPause)
        {
            PauseUnPause();
        }
    }

    public void PauseUnPause()
    {
        if (!pauseScreen.activeInHierarchy && !isPaused)
        {
            pauseScreen.SetActive(true);
            isPaused = true;
            Time.timeScale = 0;

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(resumeButton);
        }
        else
        {
            pauseScreen.SetActive(false);
            isPaused = false;
            Time.timeScale = 1;

            CloseControls();
            CloseOptions();
            CloseCredits();
        }
    }

    public void ChangeActiveButtons(int buttonToChoose)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(buttons[buttonToChoose]);
    }

    public void RestartLevel()
    {
        ChangeTimeScale();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenControls()
    {
        controlsMenu.alpha = 1;
        controlsMenu.blocksRaycasts = true;
    }

    public void CloseControls()
    {
        controlsMenu.alpha = 0;
        controlsMenu.blocksRaycasts = false;
    }

    public void OpenOptions()
    {
        optionsMenu.alpha = 1;
        optionsMenu.blocksRaycasts = true;
    }

    public void CloseOptions()
    {
        optionsMenu.alpha = 0;
        optionsMenu.blocksRaycasts = false;
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
