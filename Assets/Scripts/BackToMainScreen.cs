using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackToMainScreen : MonoBehaviour
{
    public Image fadeOut;

    private bool hiding;

    void Start()
    {
        StartCoroutine(HideImage());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            StartCoroutine(ShowImage());
        }
    }

    public IEnumerator ShowImage()
    {
        hiding = false;
        while (fadeOut.fillAmount < 1 && !hiding)
        {
            fadeOut.fillAmount += 0.01f;
            yield return new WaitForFixedUpdate();
        }
        SceneManager.LoadScene("StartMenu");
    }

    public IEnumerator HideImage()
    {
        hiding = true;
        while (fadeOut.fillAmount > 0 && hiding)
        {
            fadeOut.fillAmount -= 0.01f;
            yield return new WaitForFixedUpdate();
        }
    }
}
