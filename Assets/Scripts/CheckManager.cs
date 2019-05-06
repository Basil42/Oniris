using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum checkContent
{
    Dash_fragment= 0,
    Double_jump =1,
    Blink =2,
    Wall_Jump = 3
}
public class CheckManager : MonoBehaviour
{

    public checkContent[] content;
    bool[] assignedStatus;

    [Tooltip("Set to 0 unless you need to test sometingand you know what you are doing.")]
    public int dashFragCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        assignedStatus = new bool[content.Length];
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
        return content[index];
    }
}
