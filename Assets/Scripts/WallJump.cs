using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(CharacterController))]
public class WallJump : MonoBehaviour
{
    //this is a temporary implementation
    PlayerMovement m_movementScript;
    CharacterController m_controlller;
    [Tooltip("Minimal height around the player's transform the wall needs to cover for them to stick to it")]
    [SerializeField] private float m_minimalWallHeightFactor = 0.5f;
    private float m_reach;//how far the raycasts will "feel" for the wall, higher values will allow to wall run on walls of different incline.
    private void Awake()
    {
        m_movementScript = GetComponent<PlayerMovement>();
        m_controlller = GetComponent<CharacterController>();
        m_controlller.radius
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //check if in falling or jumping state
        //check if the collisionis with a wall, in this case, any vertical surface(with some leeway)
        Vector3 direction = hit.point - transform.position;
        direction = new Vector3(direction.x, 0.0f, direction.z);
        float angle = Vector3.SignedAngle(-direction, hit.normal, Vector3.up);
        if(direction.magnitude > m_controlller.radius){
            if (Physics.Raycast(transform.position + Vector3.up * m_minimalWallHeightFactor * m_controlller.height, direction, m_reach) &&
            Physics.Raycast(transform.position - Vector3.up * m_minimalWallHeightFactor * m_controlller.height, direction, m_reach))
                InitiateWallRun(angle);
        }
        
        
    }

    private void InitiateWallRun(float angle)
    {
        if(angle > 20.0f)
        {
            Debug.Log("wall on left");

        }else if(angle < -20.0f)
        {
            Debug.Log("wallon right");
        }
        else
        {
            Debug.Log("Frontal collision");
        }
    }
}
