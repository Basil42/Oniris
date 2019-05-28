using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadByString : MonoBehaviour
{

    public Image fadeOut;

    public void loadScene(string SceneName)
    {
       // StartCoroutine("LoadYourAsyncScene", SceneName);
        StartCoroutine(ShowImage(SceneName));
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

    public IEnumerator ShowImage(string newScene)
    {
        fadeOut.gameObject.SetActive(true);
        while (fadeOut.fillAmount < 1)
        {
            fadeOut.fillAmount += 0.01f;
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine("LoadYourAsyncScene", newScene);
    }

}
