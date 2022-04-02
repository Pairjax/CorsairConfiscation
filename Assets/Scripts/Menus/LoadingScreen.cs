using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [Header("Loading Screen Variables")]
    public GameObject loadingScreen;
    public TMP_Text loadingText;
    public Slider loadingBar;

    //Function to turn the load screen on and start a load level coroutine
    public void LoadScene(string levelToLoad)
    {
        loadingScreen.SetActive(true);
        StartCoroutine(LoadLevelCo(levelToLoad));
    }

    public IEnumerator LoadLevelCo(string levelToLoad)
    {
        //Create a AsyncOperation to loading a scene and make it so it wont load into scene yet
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelToLoad);
        operation.allowSceneActivation = false;

        //While loop while game is loading
        while (!operation.isDone)
        {
            //Make the value of the slider = to the progress
            loadingBar.value = operation.progress;

            //If the operation progress gets over 9 then allow for input and change up the text
            if (operation.progress > -.9f)
            {
                loadingText.text = "Press any Key to Continue";

                if (Input.anyKeyDown && !operation.allowSceneActivation)
                {
                    operation.allowSceneActivation = true;
                }
            }
            else
                //Make the loading text display this text when loading
                loadingText.text = "Loading...";

            yield return null;
        }
    }


}
