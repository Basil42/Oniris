using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//What should this code do? 
//It should reveal the character image on a trigger
//It should write in text in the text box

public class DialougeSystem : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI textBubble;
    private float targetPosition;
    private float dialogueStep = 0.01f;
    private bool endDialogue = false;

    private void Start()
    {
        targetPosition = textBubble.transform.localPosition.x - 186;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)) //Placeholder
        {
            StartDialogue("Hello There");
        }
    }

    public IEnumerator ShowImage()
    {
        //Lerp the image into view?
        //Mathf.Lerp(CurrentPosition , targetPosition, step);
        while(image.fillAmount < 1)
        {
            image.fillAmount += 0.01f;
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(HideImage());
    }

    public IEnumerator HideImage()
    {
        //Lerp the image out of view?
        while (image.fillAmount > 0)
        {
            image.fillAmount -= 0.01f;
            yield return new WaitForFixedUpdate();
        }
        endDialogue = true;
    }

    public void StartDialogue(string dialogue)
    {
        endDialogue = false;
        textBubble.text = dialogue;
        StartCoroutine(ShowImage());
        StartCoroutine("RunDialogue", "Hello There");
        //IEnumerate over the dialouge and update it over time?
    }

    private IEnumerator RunDialogue(string dialouge)
    {
        // GetComponentInChildren<RectTransform>().rect.position;
        dialogueStep = 0.01f;
        while (!endDialogue/*textBubble.transform.localPosition.x > targetPosition*/)
        {
            textBubble.transform.localPosition = new Vector3(Mathf.Lerp(textBubble.transform.localPosition.x, targetPosition, dialogueStep), textBubble.transform.localPosition.y, textBubble.transform.localPosition.z);
            dialogueStep += 0.001f;
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(EndDialogue());
    }

    private IEnumerator EndDialogue()
    {
        print("EndingDialogue");
        dialogueStep = 0.01f;
        while (dialogueStep < 0.3f && endDialogue/*textBubble.transform.localPosition.x < targetPosition*/)
        {
            print(dialogueStep);
            textBubble.transform.localPosition = new Vector3(Mathf.Lerp(textBubble.transform.localPosition.x, -200, dialogueStep), textBubble.transform.localPosition.y, textBubble.transform.localPosition.z);
            dialogueStep += 0.001f;
            yield return new WaitForFixedUpdate();
        }
    }
}
