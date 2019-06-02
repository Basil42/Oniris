using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public enum checkContent
{
    Dash_fragment= 0,
    Double_jump =1,
    Blink =2,
    Wall_Jump = 3
}
public class CheckManager : MonoBehaviour
{
    public TextPrompt EmptyChecksTextPrompt;
    public TextPrompt DoubleJumpTextPrompt;
    public TextPrompt BlinkTextPrompt;
    public TextPrompt WallJumpTextPrompt;
    private int[] content;
    bool[] assignedStatus;

    [Tooltip("Set to 0 unless you need to test someting and you know what you are doing.")]
    public int dashFragCount = 0;

    [DllImport("Oniris_Randomizer")]
    static public extern void Randomizer(int[] content, int size);
    private void Awake()
    {
        
        content = new int[17];
        content[0] = (int)checkContent.Double_jump;
        content[1] = (int)checkContent.Blink;
        content[2] = (int)checkContent.Wall_Jump;
        assignedStatus = new bool[17];
        //run randomizer
        Randomizer(content, content.Length);
    }
    // Start is called before the first frame update
    void Start()
    {
        
        for(int i = 0; i < content.Length; i++)
        {
            assignedStatus[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public checkContent getContent(int index, out string textPrompt)
    {
        if (assignedStatus[index]) Debug.LogError("multiple checks with identical indexes detected");

        switch ((checkContent)content[index])
        {
            case checkContent.Dash_fragment:
                textPrompt = EmptyChecksTextPrompt.texts[Random.Range(0, EmptyChecksTextPrompt.texts.Length)];
                break;
            case checkContent.Double_jump:
                textPrompt = DoubleJumpTextPrompt.texts[0];
                break;
            case checkContent.Blink:
                textPrompt = BlinkTextPrompt.texts[0];
                break;
            case checkContent.Wall_Jump:
                textPrompt = WallJumpTextPrompt.texts[0];
                break;
            default:
                textPrompt = " ";
                break;
        }
        return (checkContent)content[index];

    }
}
