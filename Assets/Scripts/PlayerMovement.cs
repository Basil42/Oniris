﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//This script is responsible for ground movement and for calling the update functions of each other state
public enum movementState
{
    grounded,
    falling,
    jumping,
    doubleJumping,
    dashing,
    wallrunLeft,
    wallrunRight,
    wallrunFront,
    blinking,
    offLedge //state in which the player is falling but can still jump when stepping off a ledge
}

[Flags]
public enum AbilityAvailability
{
    All = 0,
    doubleJump= 1,
    blink = 2,
    dash = 4,
    ledgeJump = 8,
    hasBlink = 16,
    hasDoublejump =32,
    hasDash = 64,
    hasWallJump = 128
}

[RequireComponent(typeof (CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour 
{
    [HideInInspector] public CharacterController controller;
    public float m_RunningSpeed = 1.0f;//the walk animation runs if the input vector is small enough
    public float m_SteeringSpeed = 0.2f;
    public float m_gravity = 0.3f;




    private bool m_airSwitch = false;
    public Vector3 m_inputvector;

    [HideInInspector] public movementState m_state;
    public AbilityAvailability m_abilityFlags;

    [HideInInspector] public Vector3 MovementVector;
    private Animator m_animator;


    private float colliderHeight;
    public float colliderHeightOffset = 0.2f;



    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        colliderHeight = GetComponent<CharacterController>().height - colliderHeightOffset;
      
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_state == movementState.grounded || m_state == movementState.falling) GroundCheck();
        switch (m_state)
        {
            case movementState.falling:
                fallingBehavior();
                break;
            case movementState.grounded:
                Move(m_inputvector);
                break;
            
                
            default:
                break;
        }
        Abilityresets();
        if (controller.enabled) controller.Move(MovementVector);
    }

    
    private void Abilityresets()
    {
        if (m_state == movementState.grounded)
        {

            m_abilityFlags |= AbilityAvailability.doubleJump;//sets double jump to available
        }
    }

    public void Move(Vector3 inputVector)
    {

        if (m_state == movementState.grounded)
        {
            m_airSwitch = false;
            //Lerp'n Slerp towards a target velocity
            MovementVector.x = Mathf.Lerp(MovementVector.x, inputVector.x * m_RunningSpeed, 0.08f);
            MovementVector.z = Mathf.Lerp(MovementVector.z, inputVector.z * m_RunningSpeed, 0.08f);

            if (Vector3.Scale(MovementVector,new Vector3(1.0f,0.0f,1.0f)).magnitude > 0.05f) transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(new Vector3(MovementVector.x,0.0f,MovementVector.z), Vector3.up), m_SteeringSpeed);
        }
        else
        {


            //put airborne behavior here
            
            if(m_airSwitch)//remove that
            {
                m_airSwitch = false;
                MovementVector.x = inputVector.x * m_RunningSpeed;
                MovementVector.z = inputVector.z * m_RunningSpeed;
            }
        }


        if (MovementVector.y < -1.0f && m_state == movementState.grounded)
        {
            
            MovementVector.y = -1.0f;//arbitrary value to keep it from growing ever bigger
           
            Debug.Log("Physics movement vector " + MovementVector.y);
        }

        
    }

    public void GroundCheck()
    {
        RaycastHit hit;
        RaycastHit hit2;
        RaycastHit hit3;
        RaycastHit hit4;
        RaycastHit hit5;
       

        if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), transform.TransformDirection(Vector3.down), out hit, 0.1f) |
            Physics.Raycast(transform.position + (transform.forward * 0.5f + transform.up * 0.1f), transform.TransformDirection(Vector3.down), out hit2, 0.3f) |
            Physics.Raycast(transform.position + (-transform.forward * 0.5f + transform.up * 0.1f), transform.TransformDirection(Vector3.down), out hit3, 0.3f) |
            Physics.Raycast(transform.position + (transform.right * 0.3f + transform.up * 0.1f), transform.TransformDirection(Vector3.down), out hit4, 0.3f) |
            Physics.Raycast(transform.position + (-transform.right * 0.3f + transform.up * 0.1f), transform.TransformDirection(Vector3.down), out hit5, 0.3f)
           )
        {
            Debug.DrawRay(transform.position + new Vector3(0, 0.1f, 0), transform.TransformDirection(Vector3.down) * hit2.distance, Color.yellow);
            Debug.DrawRay(transform.position + (transform.forward * 0.5f + transform.up * 0.1f), transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
            Debug.DrawRay(transform.position + (-transform.forward * 0.5f + transform.up * 0.1f), transform.TransformDirection(Vector3.down) * hit.distance, Color.green);
            Debug.DrawRay(transform.position + (transform.right * 0.3f + transform.up * 0.1f), transform.TransformDirection(Vector3.down) * hit.distance, Color.blue);
            Debug.DrawRay(transform.position + (-transform.right * 0.3f + transform.up * 0.1f), transform.TransformDirection(Vector3.down) * hit.distance, Color.black);
            
            m_state = movementState.grounded;
        }
        else
        {
            Debug.DrawRay(transform.position + new Vector3(0, 0.1f, 0), transform.TransformDirection(Vector3.down) * 1000, Color.white);
            Debug.DrawRay(transform.position + (transform.forward * 0.5f + transform.up * 0.1f), transform.TransformDirection(Vector3.down) * 1000, Color.white);
            Debug.DrawRay(transform.position + (-transform.forward * 0.5f + transform.up * 0.1f), transform.TransformDirection(Vector3.down) * 1000, Color.white);
            Debug.DrawRay(transform.position + (transform.right * 0.3f + transform.up * 0.1f), transform.TransformDirection(Vector3.down) * 1000, Color.white);
            Debug.DrawRay(-transform.position + (transform.right * 0.3f + transform.up * 0.1f), transform.TransformDirection(Vector3.down) * 1000, Color.white);
            m_state = movementState.falling;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.point.y >= transform.position.y + colliderHeight && MovementVector.y > 0)
        {
                MovementVector.y = 0;
        }
    }

    

    public void airSwitch()
    {
        m_airSwitch = true; //sets true for a function in Move() for now
    }
    public void fallingBehavior()
    {
        MovementVector.y -= m_gravity * Time.fixedDeltaTime;
}
    
}
