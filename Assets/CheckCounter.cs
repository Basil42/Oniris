using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class CheckCounter : MonoBehaviour
{
    public int requiredChecks;
    private int collectedChecks = 0;
    private int totalChecks;
    private TextMeshProUGUI counter;

    public Image fadeOut;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] checks = GameObject.FindGameObjectsWithTag("Check");
        totalChecks = checks.Length;
        counter = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addCheck()
    {
        collectedChecks++;
        counter.text = collectedChecks +"/"+ (totalChecks - 3); //Hard code for now
        if(collectedChecks >= requiredChecks)
        {
            StartCoroutine(end());
            GameObject.FindGameObjectWithTag("DialogueSystem").GetComponent<DialougeSystem>().StartDialogue("I think I have enough orbs now. I should head back to the start", 7);
        }
    }

    private IEnumerator end()
    {
        while(fadeOut.color.a < 1)
        {
            var tempcolor = fadeOut.color;
            tempcolor.a += 0.0f;
            fadeOut.color = tempcolor;
            yield return new WaitForFixedUpdate();
        }
        //SceneManager.LoadSceneAsync();
    }
}
