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

    public CinemachineFreeLook lastCamera;

    public Transform PHplayer;

    private bool PHcamBlinking = false;
 
    //Testing to see if this works
    private void Awake()
    {
        Application.targetFrameRate = 60;
        PHplayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CamSwitchBlink()
    {
        //Turn off current camera, move to next camera, turn on last camera
        //Another idea is to make a camera follow a rail in the direction of blink

        if (!PHcamBlinking)
        {
            lastCamera.m_Follow = PHplayer;
            lastCamera.m_LookAt = PHplayer;
            currentCamera.m_Follow = null;
            currentCamera.m_LookAt = null;

            //Give next camera the rotation and altitude of the current camera (Maybe just y position)
            //Y position, rotation... 

            //State transition
            cameraStateController.SetBool("Switch", true);
            Debug.Log("camBlinking");
            PHcamBlinking = true;
        }
        else
        {
            currentCamera.m_Follow = PHplayer;
            currentCamera.m_LookAt = PHplayer;
            lastCamera.m_Follow = null;
            lastCamera.m_LookAt = null;

            //State transition
            cameraStateController.SetBool("Switch", false);
            PHcamBlinking = false;
        }
        
        
    }

}
