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

    public checkContent[] content;
    bool[] assignedStatus;

    [Tooltip("Set to 0 unless you need to test sometingand you know what you are doing.")]
    public int dashFragCount = 0;
    // Start is called before the first frame update
    [DllImport("Oniris_Randomizer")]
    private static extern void Randomizer(int[] p_content, int size);
    void Start()
    {
        assignedStatus = new bool[content.Length];
        
        int[] convertedArray = System.Array.ConvertAll(content, value => (int)value);
        foreach (int item in convertedArray)
        {
            Debug.Log(item);
        }
        Randomizer(convertedArray, convertedArray.Length);
        foreach (int item in convertedArray)
        {
            Debug.Log(item);
        }
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
