using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public RawImage WhiteImage;
    // Start is called before the first frame update
    void Start()
    {
        WhiteImage.CrossFadeAlpha(0.0f, 3.0f, true);
        StartCoroutine(CreditRoll());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator CreditRoll()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("Credits");
        load.allowSceneActivation = false;
        yield return new WaitUntil(() => Input.anyKey);
        load.allowSceneActivation = true;
    }
}
