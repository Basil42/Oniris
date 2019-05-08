using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
    

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform.position,Vector3.up);
    }
}