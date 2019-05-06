using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//The purpose of this script is to handle the cinemachine camera. Making sure it works properly during 
//moves such as blink or certain parts of the level.

public class CameraController : MonoBehaviour
{

    public Animator cameraStateController;

    public GameObject stateDrivenCamera;

    public CinemachineFreeLook currentCamera; 

   // public CinemachineFreeLook lastCamera;

    public GameObject PHplayer;

    //private bool PHcamBlinking = false;
 
    //Testing to see if this works
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }



    //public void CamSwitchBlink()
    //{
    //    //Turn off current camera, move to next camera, turn on last camera
    //    //Another idea is to make a camera follow a rail in the direction of blink

    //    if (!PHcamBlinking)
    //    {
    //        lastCamera.m_Follow = PHplayer.transform;
    //        lastCamera.m_LookAt = PHplayer.transform;
    //        currentCamera.m_Follow = null;
    //        currentCamera.m_LookAt = null;

    //        //State transition
    //        cameraStateController.SetBool("Switch", true);
    //        Debug.Log("camBlinking");
    //        PHcamBlinking = true;
    //    }
    //    else
    //    {
    //        currentCamera.m_Follow = PHplayer.transform;
    //        currentCamera.m_LookAt = PHplayer.transform;
    //        lastCamera.m_Follow = null;
    //        lastCamera.m_LookAt = null;

    //        //State transition
    //        cameraStateController.SetBool("Switch", false);
    //        Debug.Log("camBlinking");
    //        PHcamBlinking = false;
    //    }
        
        
}
