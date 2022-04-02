using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio Variables")]
    public AudioMixer theMixer;
    public Slider masterSlider, musicSlider, sfxSlider;
    public TMP_Text masterValueText, musicValueText, sfxValueText;

    [Header("FPS Counter Variables")]
    public Toggle fpsToggle;
    public TMP_Text fpsText;
    public GameObject fpsCounterObject;
    [HideInInspector] public int fpsInt;

    [Header("FullScreen Variables")]
    public Toggle fullscreenToggle;
    [HideInInspector] public int fullscreenInt;

    [Header("Vsync Variables")]
    public Toggle vSyncToggle;
    [HideInInspector] public int vsyncInt;

    void Awake()
    {
        //Check if there is a key for the playerprefs for the fps counter and set the int depending on it
        if (PlayerPrefs.HasKey("FpsToggleState"))
            fpsInt = PlayerPrefs.GetInt("FpsToggleState");
        else
            fpsInt = 1;

        if (fpsInt == 1)
        {
            fpsToggle.isOn = true;
            fpsCounterObject.SetActive(true);
        }
        else
        {
            fpsToggle.isOn = false;
            fpsCounterObject.SetActive(false);
        }

        //Check if there is a key for the playerprefs for the fullscreen and set the int depending on it
        if (PlayerPrefs.HasKey("FullscreenToggleState"))
            fullscreenInt = PlayerPrefs.GetInt("FullscreenToggleState");
        else
            fullscreenInt = 1;

        if (fullscreenInt == 1)
        {
            fullscreenToggle.isOn = true;
            Screen.fullScreen = true;
        }
        else
        {
            fullscreenToggle.isOn = false;
            Screen.fullScreen = false;
        }

        //Check if there is a key for the playerprefs for the vsync and set the int depending on it
        if (PlayerPrefs.HasKey("VsyncToggleState"))
            vsyncInt = PlayerPrefs.GetInt("VsyncToggleState");
        else
            vsyncInt = 1;

        if (vsyncInt == 1)
        {
            vSyncToggle.isOn = true;
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            fullscreenToggle.isOn = false;
            QualitySettings.vSyncCount = 0;
        }

    }

    void Start()
    {
        //Check if the master volume has a key via playerprefs and adjusts it value according to value stored in key
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            theMixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVolume"));
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        }

        //Check if the music volume has a key via playerprefs and adjusts it value according to value stored in key
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            theMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVolume"));
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }

        //Check if the sfx volume has a key via playerprefs and adjusts it value according to value stored in key
        if (PlayerPrefs.HasKey("SfxVolume"))
        {
            theMixer.SetFloat("SfxVol", PlayerPrefs.GetFloat("SfxVolume"));
            sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume");
        }

        //Adjust the value of the value texts to the right of the slider using the value stored in playerprefs
        masterValueText.text = (masterSlider.value + 80).ToString() + "%";
        musicValueText.text = (musicSlider.value + 80).ToString() + "%";
        sfxValueText.text = (sfxSlider.value + 80).ToString() + "%";
    }

    void Update()
    {
        //Calculating the FPS
        float fps = 1 / Time.unscaledDeltaTime;
        fpsText.text = "FPS: " + fps.ToString("F0");
    }

    public void AdjustFpsCounter(bool isFpsOn)
    {
        if (isFpsOn == false)
        {
            PlayerPrefs.SetInt("FpsToggleState", 0);
            fpsCounterObject.SetActive(false);
            Debug.Log("FPS Counter is Off");
        }
        else
        {
            PlayerPrefs.SetInt("FpsToggleState", 1);
            fpsCounterObject.SetActive(true);
        }
    }

    public void AdjustFullscreen(bool isFullscreenOn)
    {
        Screen.fullScreen = isFullscreenOn;

        if (isFullscreenOn == false)
        {
            isFullscreenOn = false;
            PlayerPrefs.SetInt("FullscreenToggleState", 0);
            Debug.Log("Exiting FullScreen");
        }
        else
        {
            isFullscreenOn = true;
            PlayerPrefs.SetInt("FullscreenToggleState", 1);
        }
    }

    public void AdjustVysnc(bool isVsyncOn)
    {
        if (isVsyncOn == false)
        {
            PlayerPrefs.SetInt("VsyncToggleState", 0);
            QualitySettings.vSyncCount = 0;
            Debug.Log("The Vsync is Off");
        }
        else
        {
            PlayerPrefs.SetInt("VsyncToggleState", 1);
            QualitySettings.vSyncCount = 1;
        }
    }

    //Function to adjust the master volume by readjusting the value text and slider and setting the float for playerprefs
    public void AdjustMasterVolume()
    {
        masterValueText.text = (masterSlider.value + 80).ToString() + "%";
        theMixer.SetFloat("MasterVol", masterSlider.value);
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
    }

    //Function to adjust the music volume by readjusting the value text and slider and setting the float for playerprefs
    public void AdjustMusicVolume()
    {
        musicValueText.text = (musicSlider.value + 80).ToString() + "%";
        theMixer.SetFloat("MusicVol", musicSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    //Function to adjust the sfx volume by readjusting the value text and slider and setting the float for playerprefs
    public void AdjustSfxVolume()
    {
        sfxValueText.text = (sfxSlider.value + 80).ToString() + "%";
        theMixer.SetFloat("SfxVol", sfxSlider.value);
        PlayerPrefs.SetFloat("SfxVolume", sfxSlider.value);
    }
}
