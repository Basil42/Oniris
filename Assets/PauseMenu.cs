using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPaused = false;
    public GameObject pauseMenu;
    public Selectable resumeButton;
    public Selectable quitButton;

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

    public void ResumeTime()
    {
        GameIsPaused = false;
        
    }

    public void QuitGame()
    {
        Debug.Log("Quiting Game");
        Application.Quit();
    }
}
