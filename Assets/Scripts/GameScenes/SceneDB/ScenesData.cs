using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

[CreateAssetMenu(fileName = "sceneDB", menuName = "Scene Data/Database")]
public class ScenesData : ScriptableObject
{
    public List<Level> levels = new List<Level>();
    public string currentLevelName;

    public Level currentLevel;

    public void Awake()
    {
        currentLevelName = currentLevel.sceneName;
    }

    public void LoadLevel(string sceneName)
    {
        if (levels.Any(x => x.sceneName == sceneName))
        {
            SceneManager.LoadSceneAsync(sceneName);
            currentLevelName = sceneName;
        }

        else
            Debug.Log($"Specified scene {sceneName} does not exist and could not be loaded! Double check the name");
    }

    public Level PickRandomLevel()
    {
        return levels.ElementAt(UnityEngine.Random.Range(0, levels.Count - 1));
    }

}
