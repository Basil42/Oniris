using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadByString : MonoBehaviour
{
    
    public void loadScene(string SceneName)
    {
        StartCoroutine("LoadYourAsyncScene", SceneName);
    }

    IEnumerator LoadYourAsyncScene(string SceneName)
    {
        // The Application loads the Scene in the background as the current Scene runs.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

}
