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

    // Levels

    public void LoadLevel(string sceneName)
    {
        if (levels.Any(x => x.sceneName == sceneName))
        {
            SceneManager.LoadSceneAsync(sceneName);
            currentLevelName = sceneName;
        }

        else
            Debug.Log("Specified scene does not exist and could not be loaded! Double check the name");
    }

}
