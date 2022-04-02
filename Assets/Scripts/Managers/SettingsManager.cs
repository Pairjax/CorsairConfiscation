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

    /*[Header("FPS Counter Variables")]
    public Toggle fpsToggle;
    public TMP_Text fpsText;
    public GameObject fpsCounterObject;
    [HideInInspector] public int fpsInt;*/

    void Awake()
    {

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

    }
}
