using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameScene : ScriptableObject
{

    [Header("General Info")]
    public string sceneName;
    public string description;

    [Header("Sounds")]
    public AudioClip song;
    [Range(0.0f, 1.0f)]
    public float musicVolume;
}
