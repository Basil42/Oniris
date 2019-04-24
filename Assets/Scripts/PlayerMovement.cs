﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum movementState
{
    grounded,
    falling,
    jumping,
    dashing,
    wallrunLeft,
    wallrunRight,
    wallrunFront,
    blinking,
}

[Flags]
public enum AbilityAvailability
{
    All = 0,
    doubleJumped= 1,
    blinked = 2,
    dashed = 4,
    ledgeJump = 8
}

[RequireComponent(typeof (CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour 
{
    [HideInInspector] public CharacterController controller;
    public float m_RunningSpeed = 1.0f;//the walk animation runs if the input vector is small enough
    public float m_SteeringSpeed = 0.2f;
    public float m_gravity = 10.0f;
    public float m_lowGravity = 5.0f;
    public movementState state;
    public AbilityAvailability abilityflags;
    [SerializeField] private float m_jumpingSpeed = 2.0f;
    [SerializeField] private float m_DoubleJumpSpeed = 2.0f;

    private bool m_busy = false;
    
    private bool m_doubleJumped = false;
    private bool m_airSwitch = false;
    public bool m_grounded = false;
    public bool m_jumpEnabled = true;
    public bool m_doubleJumpEnabled = false;

    [HideInInspector] public Vector3 MovementVector;
    private Animator m_animator;

    private bool m_shortJump;
    private float m_jumpTransitionTimer = 0;
    public float m_jumpTransitionLimit = 0.3f; //Limit -> Length?

    private float colliderHeight;
    public float colliderHeightOffset = 0.2f;



    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        colliderHeight = GetComponent<CharacterController>().height - colliderHeightOffset;
      
    }
    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        if (m_grounded)
        {
            m_doubleJumped = false;

        }
    }

    

    

    public void Jump()
    {
        if (m_grounded)
        {
            MovementVector.y = m_jumpingSpeed;
           

            m_shortJump = false;
            m_jumpTransitionTimer = 0;
            
        } else if (!m_doubleJumped)
        {
            MovementVector.y = m_DoubleJumpSpeed;
            

            m_doubleJumped = true;
            m_shortJump = false;
            m_jumpTransitionTimer = 0;
        }

    }

    public void ShortJump()
    {
        // could do this another way: apply force if getbuttonDown, if not stop applying force

        if (m_jumpTransitionTimer < m_jumpTransitionLimit && Input.GetButtonUp("Jump"))//need tobe moved out
        {
            m_shortJump = true;
        }

        if (m_shortJump && !m_busy)
        {
            MovementVector.y = MovementVector.y - (m_gravity * 4 * Time.fixedDeltaTime);

        }
        else if (!m_busy)
        {
            MovementVector.y = MovementVector.y - (m_gravity * Time.fixedDeltaTime);
            m_jumpTransitionTimer += Time.fixedDeltaTime;

        }

        if (m_grounded)
        {
            m_shortJump = false;

        }
    }

    public void Move(Vector3 inputVector)
    {

        if (m_grounded && !m_busy)
        {
            m_doubleJumped = false;
            m_airSwitch = false;
            //Lerp'n Slerp towards a target velocity
            MovementVector.x = Mathf.Lerp(MovementVector.x, inputVector.x * m_RunningSpeed, 0.08f);
            MovementVector.z = Mathf.Lerp(MovementVector.z, inputVector.z * m_RunningSpeed, 0.08f);

            if (MovementVector.magnitude > 0.05f) transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(new Vector3(MovementVector.x,0.0f,MovementVector.z), Vector3.up), m_SteeringSpeed);
        }
        else
        {


            //put airborne behavior here

            if(m_doubleJumped && !m_airSwitch)
            {
                m_airSwitch = true;

                MovementVector.x = inputVector.x * m_RunningSpeed;
                MovementVector.z = inputVector.z * m_RunningSpeed;
            }

            //Short jump behaviour

            ShortJump();//will be called by the input manager,like the rest
        }


        if (MovementVector.y < -1.0f && m_grounded)
        {
            
            MovementVector.y = -1.0f;//arbitrary value to keep it from growing ever bigger
           
            Debug.Log("Physics movement vector " + MovementVector.y);
        }

        if(controller.enabled)controller.Move(MovementVector);
    }

    private void GroundCheck()
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

            m_grounded = true;
        }
        else
        {
            Debug.DrawRay(transform.position + new Vector3(0, 0.1f, 0), transform.TransformDirection(Vector3.down) * 1000, Color.white);
            Debug.DrawRay(transform.position + (transform.forward * 0.5f + transform.up * 0.1f), transform.TransformDirection(Vector3.down) * 1000, Color.white);
            Debug.DrawRay(transform.position + (-transform.forward * 0.5f + transform.up * 0.1f), transform.TransformDirection(Vector3.down) * 1000, Color.white);
            Debug.DrawRay(transform.position + (transform.right * 0.3f + transform.up * 0.1f), transform.TransformDirection(Vector3.down) * 1000, Color.white);
            Debug.DrawRay(-transform.position + (transform.right * 0.3f + transform.up * 0.1f), transform.TransformDirection(Vector3.down) * 1000, Color.white);
            m_grounded = false;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.point.y >= transform.position.y + colliderHeight && MovementVector.y > 0)
        {
                MovementVector.y = 0;
        }
    }

    public void setBusy(bool boolean)
    {
        m_busy = boolean;
    }

    public bool getBusy()
    {
        return m_busy;
    }

}
