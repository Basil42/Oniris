using System.Collections;
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
    doubleJump = 1,
    blink = 2,
    dash = 4,
    ledgeJump = 8,
    hasBlink = 16,
    hasDoublejump = 32,
    hasDash = 64,
    hasWallJump = 128
}

[RequireComponent(typeof (CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour 
{
    public const float inertiaIntensity = 0.08f;
    [HideInInspector] public CharacterController controller;
    public float m_RunningSpeed = 1.0f;//the walk animation runs if the input vector is small enough
    public float m_SteeringSpeed = 0.2f;
    public float m_AirSteeringSpeed = 0.2f;
    public float m_AirForwardSpeed = 0.2f;
    public const float m_AirInertiaIntensity = 0.08f;
    public float m_AirMinSpeed = 0.01f; //Need a better name
    public float m_gravity = 0.3f;


    public Vector3 m_inputvector;

    [HideInInspector] public movementState m_state;
    [HideInInspector]public AbilityAvailability m_abilityFlags;

    [HideInInspector] public Vector3 MovementVector;
    private Animator m_animator;


    private float colliderHeight;
    public float colliderHeightOffset = 0.1f;

    [Header("debuging functions")]
    public bool StartWithBlink;
    public bool StartWithDoubleJump;
    public bool StartWithWallJump;
    public bool StartWithDash;



    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        colliderHeight = GetComponent<CharacterController>().height - colliderHeightOffset;

        if (StartWithBlink) m_abilityFlags |= AbilityAvailability.hasBlink;
        if (StartWithDash) m_abilityFlags |= AbilityAvailability.hasDash;
        if (StartWithDoubleJump) m_abilityFlags |= AbilityAvailability.hasDoublejump;
        if (StartWithWallJump) m_abilityFlags |= AbilityAvailability.hasWallJump;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_state == movementState.grounded || m_state == movementState.falling) GroundCheck();
        switch (m_state)
        {
            case movementState.falling:
                fallingBehavior();
                airControl(m_inputvector);
                break;
            case movementState.grounded:
                Move(m_inputvector);
                break;
            case movementState.jumping:
                airControl(m_inputvector);
                break;
            case movementState.doubleJumping:
                airControl(m_inputvector);
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
            m_abilityFlags |= AbilityAvailability.blink;
            m_abilityFlags |= AbilityAvailability.dash;
           
        }
    }

    public void Move(Vector3 inputVector)
    {
            //Lerp'n Slerp towards a target velocity
            MovementVector.x = Mathf.Lerp(MovementVector.x, inputVector.x * m_RunningSpeed, inertiaIntensity);
            MovementVector.z = Mathf.Lerp(MovementVector.z, inputVector.z * m_RunningSpeed, inertiaIntensity);

            if (Vector3.Scale(MovementVector,new Vector3(1.0f,0.0f,1.0f)).magnitude > 0.05f) transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(new Vector3(MovementVector.x,0.0f,MovementVector.z), Vector3.up), m_SteeringSpeed);

        if (MovementVector.y < -1.0f)
        {
            
            MovementVector.y = -1.0f;//arbitrary value to keep it from growing ever bigger
           
            
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
            m_state = movementState.falling;
        }
    }

    private void airControl(Vector3 inputVector)
    {   
        
        Vector3 airInput = Vector3.Project(inputVector, transform.forward) * m_AirForwardSpeed;

        airInput += Vector3.Project(inputVector, transform.right)* m_AirSteeringSpeed;

        if (Vector3.Dot(MovementVector, transform.forward) > m_AirMinSpeed) //If the value is too high, manuevering doesnt happen
        {
            MovementVector.z = Mathf.Lerp(MovementVector.z, airInput.z, m_AirInertiaIntensity);
            MovementVector.x = Mathf.Lerp(MovementVector.x, airInput.x, m_AirInertiaIntensity);
        }
        
    }

    
    public void fallingBehavior()
    {
        MovementVector.y -= m_gravity * Time.fixedDeltaTime;
    }
    
}
