using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//What should this code do? 
//It should reveal the character image on a trigger
//It should write in text in the text box

    //TODO: improve lerp, possibly do another way
public class DialougeSystem : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI textBubble;
    private float targetPosition;
    private float targetOffset = 200.0f;
    private float dialogueStep = 0.01f;
    private float imageStep = 0.01f;
    private bool runningDialogue = false;

    private void Start()
    {
        targetPosition = textBubble.transform.localPosition.x - targetOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)) //Placeholder
        {
            StartDialogue("Hello There",5);
        }
    }

    public void StartDialogue(string dialogue, float time)
    {
        if (!runningDialogue)
        {
            runningDialogue = true;
            textBubble.text = dialogue;
            StartCoroutine(ShowImage());
            StartCoroutine("RunDialogue");
            Invoke("StopDialogue", time);
        }
    }

    //Moves image into view as long as dialogue is running, then starts HideImage
    public IEnumerator ShowImage()
    {
        imageStep = 0.01f;
        while (runningDialogue)
        {
           // image.fillAmount += 0.01f;
            //image.transform.localPosition = new Vector3(Mathf.Lerp(image.transform.localPosition.x, -360, imageStep), image.transform.localPosition.y, image.transform.localPosition.z);
            imageStep += 0.001f;
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(HideImage());
    }

    //While not running, move image out of view, end after a little while or when a new dialogue starts running
    public IEnumerator HideImage()
    {
        imageStep = 0.01f;
        while (imageStep < 0.3f && !runningDialogue)
        {
            //image.fillAmount -= 0.01f;
            //image.transform.localPosition = new Vector3(Mathf.Lerp(image.transform.localPosition.x, -200, imageStep), image.transform.localPosition.y, image.transform.localPosition.z);
            imageStep += 0.001f;
            yield return new WaitForFixedUpdate();
        }
    }

    //Moves dialogue into view, also transitions into EndDialogue when no longer runningDialogue is true
    private IEnumerator RunDialogue()
    {
        dialogueStep = 0.01f;
        while (runningDialogue)
        {
            textBubble.transform.localPosition = new Vector3(Mathf.Lerp(textBubble.transform.localPosition.x, targetPosition, dialogueStep), textBubble.transform.localPosition.y, textBubble.transform.localPosition.z);
            dialogueStep += 0.001f;
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(EndDialogue());
    }

    //While not running, move text out of view, end after a little while or when a new dialogue starts running
    private IEnumerator EndDialogue()
    {
        print("EndingDialogue");
        dialogueStep = 0.01f;
        while (dialogueStep < 0.3f && !runningDialogue)
        {
            print(dialogueStep);
            textBubble.transform.localPosition = new Vector3(Mathf.Lerp(textBubble.transform.localPosition.x, -200, dialogueStep), textBubble.transform.localPosition.y, textBubble.transform.localPosition.z);
            dialogueStep += 0.001f;
            yield return new WaitForFixedUpdate();
        }
    }

    private void StopDialogue() //Meant to be invoked as a timer for the dialogue
    {
        runningDialogue = false;
    }
}
