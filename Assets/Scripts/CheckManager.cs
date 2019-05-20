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

    private int[] content;
    bool[] assignedStatus;

    [Tooltip("Set to 0 unless you need to test sometingand you know what you are doing.")]
    public int dashFragCount = 0;

    [DllImport("Oniris_Randomizer")]
    static public extern void Randomizer(int[] content, int size);
    private void Awake()
    {
        content = new int[69];
        content[0] = (int)checkContent.Double_jump;
        content[1] = (int)checkContent.Blink;
        content[2] = (int)checkContent.Wall_Jump;
        assignedStatus = new bool[69];
        //run randomizer
        Randomizer(content, content.Length);
    }
    // Start is called before the first frame update
    void Start()
    {
        
        //for(int i = 0; i < content.Length; i++)
        //{
        //    assignedStatus[i] = false;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public checkContent getContent(int index)
    {
        if (assignedStatus[index]) Debug.LogError("multiple checks with identical indexes detected");
        return (checkContent)content[index];

    }
}
