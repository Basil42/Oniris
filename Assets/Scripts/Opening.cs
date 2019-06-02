using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Opening : MonoBehaviour
{
    public RawImage Background;
    public RawImage Frame1;
    public RawImage Frame2;
    public RawImage Frame3;
    public RawImage CursorFrame;
    public RawImage BlackFrame;
    public TextMeshProUGUI[] InText;
    public float TextFadeTime = 1.5f;
    private float internalTime = 0.0f;
    public float frameDuration = 2.0f;
    public float CrossfadeDuration = 1.5f;
    public float CursorFrequency = 0.8f;
    public float Sceneduration = 10.0f;
    public float SceneFadeInSpeed = 3.0f;
    public float SceneFadeOutSpeed = 4.0f;

    private void Awake()
    {
        Frame2.CrossFadeAlpha(0.0f, 0.0f, true);
        Frame3.CrossFadeAlpha(0.0f, 0.0f, true);
        foreach (TextMeshProUGUI text in InText)
        {
            text.alpha = 0.0f;
            
        }
    }
    void Start()
    {
        StartCoroutine(SceneRoll());
    }

    private void Update()
    {
        internalTime += Time.deltaTime;
        CursorFrame.CrossFadeAlpha((Mathf.Sin(internalTime / CursorFrequency) > 0) ? 0.0f : 1.0f, 0.0f, true);
    }

    IEnumerator SceneRoll()
    {
        foreach (TextMeshProUGUI text in InText)
        {
            float timer = 0.0f;
            while(timer < 1.0f)
            {
                timer += Time.deltaTime / TextFadeTime;
                text.alpha = Mathf.Lerp(0, 1, timer);
                yield return null;
            }
            
        }
        
        BlackFrame.CrossFadeAlpha(0, SceneFadeInSpeed,true);//fade in
        foreach(TextMeshProUGUI text in InText) {
            text.CrossFadeAlpha(0, SceneFadeInSpeed, true);
        }
        yield return new WaitForSecondsRealtime(SceneFadeInSpeed + frameDuration);
        //animation
        yield return new WaitForSecondsRealtime(frameDuration);
        Frame1.CrossFadeAlpha(0.0f, CrossfadeDuration, true);
        Frame2.CrossFadeAlpha(1.0f, CrossfadeDuration, true);
        yield return new WaitForSecondsRealtime(frameDuration);
        Frame2.CrossFadeAlpha(0.0f, CrossfadeDuration, true);
        Frame3.CrossFadeAlpha(1.0f, CrossfadeDuration, true);
        yield return new WaitUntil(() => internalTime > Sceneduration);
        //fade out
        BlackFrame.CrossFadeAlpha(1.0f, SceneFadeOutSpeed, true);

    }
}
