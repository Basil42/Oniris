using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//The purpose of this script is to handle the cinemachine camera. Making sure it works properly during 
//moves such as blink or certain parts of the level.

public class CameraController : MonoBehaviour
{

    public Animator cameraStateController;

    public GameObject stateDrivenCamera;

    private GameObject currentCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CamSwitchBlink()
    {
        //When the player blinks, turn off the current camera, 
        //create a new free camera in the direction of the player, swap to it.



    }

}
