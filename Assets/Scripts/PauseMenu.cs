using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPaused = false;
    public GameObject pauseMenu;
    public Selectable resumeButton;
    public Selectable restartButton;
    public Selectable quitButton;
    public Image fadeOut;

    private Scene scene;

    private void Awake()
    {
        scene = SceneManager.GetActiveScene();
        //SceneManager.activeSceneChanged += ChangedActiveScene;
        fadeOut.gameObject.SetActive(true);
        fadeOut.fillAmount = 1;
        StartCoroutine(HideImage());
    }

    //I would like to do something with this later, it would allow for different fadeouts/fadeins

    //private void ChangedActiveScene(Scene current, Scene next)
    //{
    //    print("Hello there");
    //    if (current.name == next.name)
    //    {

    //        fadeOut.gameObject.SetActive(true);
    //        fadeOut.fillAmount = 1;
    //        StartCoroutine(HideImage());
    //    }
    //}


    public void PauseResume()
    {
        if (!GameIsPaused)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void Pause()
    {
        GameIsPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        quitButton.Select(); //Quick hack to make it so the resumebutton is always visibly selected
        resumeButton.Select();
    }

    public void Resume() //I want to not jump when I press the resume button, so I delay the full resume a bit
    {
        pauseMenu.SetActive(false);
        Invoke("ResumeTime", 0.1f);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        //StartCoroutine("LoadYourAsyncScene", scene.name);
        StartCoroutine(ShowImage());
    }

    public void ResumeTime()
    {
        GameIsPaused = false;
        
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

    public void QuitGame()
    {
        Debug.Log("Quiting Game");
        Application.Quit();
    }

    public IEnumerator ShowImage()
    {
        //Mathf.Lerp(CurrentPosition , targetPosition, step);
        fadeOut.gameObject.SetActive(true);
        while (fadeOut.fillAmount < 1)
        {
            fadeOut.fillAmount += 0.01f;
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine("LoadYourAsyncScene", scene.name);
    }

    public IEnumerator HideImage()
    {
        //Lerp the image out of view?
        while (fadeOut.fillAmount > 0)
        {
            fadeOut.fillAmount -= 0.01f;
            yield return new WaitForFixedUpdate();
        }
        fadeOut.gameObject.SetActive(false);
    }
}
