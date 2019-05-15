using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Experimental.Rendering.HDPipeline;

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
    public float m_AirSteeringSpeed = 0.1f;
    public float m_AirForwardSpeed = 0.2f;
    public const float m_AirInertiaIntensity = 0.08f;
    public float m_gravity = 0.3f;

    private DecalProjectorComponent m_dropShadow;
    public float m_dropShadowFadeMultiplier = 0.1f;
    public float m_AirControlSpeedCap;
    public Vector3 m_inputvector;

    public movementState m_state;
    [HideInInspector]public AbilityAvailability m_abilityFlags;

    public Vector3 MovementVector;
    [HideInInspector] public Animator m_animator;


    private float colliderHeight;
    private float colliderHeightOffset = 0.01f;

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
        m_animator = GetComponent<Animator>();

        m_dropShadow = GameObject.FindGameObjectWithTag("dropShadow").GetComponent<DecalProjectorComponent>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        m_animator.SetBool("isGrounded", m_state == movementState.grounded);
        m_animator.SetFloat("speed", Vector3.ProjectOnPlane(MovementVector,Vector3.up).magnitude/m_RunningSpeed);
        if (m_state == movementState.grounded || m_state == movementState.falling) GroundCheck();
        switch (m_state)
        {
            case movementState.falling:
                fallingBehavior();
                airControl(m_inputvector);
                break;
            case movementState.grounded:
                //MovementVector.y = 0.2f;
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
        fadeShadow();
        if (controller.enabled) controller.Move(MovementVector);
    }

    
    private void Abilityresets()
    {
        if (m_state == movementState.grounded)
        {
            //to do at timers
            m_abilityFlags |= AbilityAvailability.doubleJump;//sets double jump to available
            m_abilityFlags |= AbilityAvailability.blink;
            m_abilityFlags |= AbilityAvailability.dash;
        }
    }

    public void Move(Vector3 inputVector)
    {
            //Lerp'n Slerp towards a target velocity
            //MovementVector.x = Mathf.Lerp(MovementVector.x, inputVector.x * m_RunningSpeed, inertiaIntensity);
            //MovementVector.z = Mathf.Lerp(MovementVector.z, inputVector.z * m_RunningSpeed, inertiaIntensity);
            MovementVector = Vector3.Lerp(MovementVector, inputVector * m_RunningSpeed, inertiaIntensity);

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
       

        if (Physics.Raycast(transform.position + new Vector3(0, 0.4f, 0), transform.TransformDirection(Vector3.down), out hit, 0.6f) ||
            Physics.Raycast(transform.position + (transform.forward * 0.5f + transform.up * 0.4f), transform.TransformDirection(Vector3.down), out hit2, 0.6f) ||
            Physics.Raycast(transform.position + (-transform.forward * 0.5f + transform.up * 0.4f), transform.TransformDirection(Vector3.down), out hit3, 0.6f) ||
            Physics.Raycast(transform.position + (transform.right * 0.3f + transform.up * 0.4f), transform.TransformDirection(Vector3.down), out hit4, 0.6f) ||
            Physics.Raycast(transform.position + (-transform.right * 0.3f + transform.up * 0.4f), transform.TransformDirection(Vector3.down), out hit5, 0.6f)
           )
        {
            Debug.DrawRay(transform.position + new Vector3(0, 0.1f, 0), transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            Debug.DrawRay(transform.position + (transform.forward * 0.5f + transform.up * 0.1f), transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
            Debug.DrawRay(transform.position + (-transform.forward * 0.5f + transform.up * 0.1f), transform.TransformDirection(Vector3.down) * hit.distance, Color.green);
            Debug.DrawRay(transform.position + (transform.right * 0.3f + transform.up * 0.1f), transform.TransformDirection(Vector3.down) * hit.distance, Color.blue);
            Debug.DrawRay(transform.position + (-transform.right * 0.3f + transform.up * 0.1f), transform.TransformDirection(Vector3.down) * hit.distance, Color.black);
            if (m_state == movementState.falling)
            {
                m_animator.SetTrigger("run");
                MovementVector = transform.forward * Vector3.ProjectOnPlane(MovementVector,Vector3.up).magnitude;
            }
            m_state = movementState.grounded;


        }
        else
        {
            Debug.DrawRay(transform.position + new Vector3(0, 0.1f, 0), transform.TransformDirection(Vector3.down) * 1000, Color.white);
            Debug.DrawRay(transform.position + (transform.forward * 0.5f + transform.up * 0.1f), transform.TransformDirection(Vector3.down) * 1000, Color.white);
            Debug.DrawRay(transform.position + (-transform.forward * 0.5f + transform.up * 0.1f), transform.TransformDirection(Vector3.down) * 1000, Color.white);
            Debug.DrawRay(transform.position + (transform.right * 0.3f + transform.up * 0.1f), transform.TransformDirection(Vector3.down) * 1000, Color.white);
            Debug.DrawRay(-transform.position + (transform.right * 0.3f + transform.up * 0.1f), transform.TransformDirection(Vector3.down) * 1000, Color.white);
            if(m_state == movementState.grounded)m_animator.SetTrigger("fall");
            m_state = movementState.falling;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.point.y >= transform.position.y + colliderHeight && MovementVector.y > 0)
        {
            MovementVector.y = 0;
            if(m_state == movementState.wallrunFront) {
                GetComponent<WallJump>().Eject();
            }
            else
            {
                m_state = movementState.falling;
            }

        }
        else
        {
            if (m_state == movementState.falling) MovementVector += hit.normal * 0.1f;
        }
       
    }

    private void airControl(Vector3 inputVector)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(m_inputvector, Vector3.up), 10.0f);
        Vector3 HorizontalMovement = Vector3.ProjectOnPlane(MovementVector, Vector3.up);
        //Add air input influence to the input vector,modified to dampen lateral influence 
        Vector3 airInput = Vector3.Project(inputVector, Vector3.ProjectOnPlane(MovementVector, Vector3.up)) * m_AirForwardSpeed;
        airInput += Vector3.Project(inputVector, Vector3.Cross(Vector3.ProjectOnPlane(MovementVector, Vector3.up),Vector3.up)) * m_AirSteeringSpeed;
        airInput += HorizontalMovement;
        
        if(airInput.magnitude > m_AirControlSpeedCap)
        {
            airInput = Vector3.ClampMagnitude(airInput, HorizontalMovement.magnitude);
        }
        MovementVector.x = airInput.x;
        MovementVector.z = airInput.z;



        //If dash would not be useable, then do not use aircontrol
        //if (m_abilityFlags.HasFlag(AbilityAvailability.dash))
        //{
        //    
        //    Vector3 airInput = Vector3.Project(inputVector, Vector3.ProjectOnPlane(MovementVector, Vector3.up)) * m_AirForwardSpeed;
        //
        //    airInput += Vector3.Project(inputVector, Vector3.Cross(Vector3.ProjectOnPlane(MovementVector, Vector3.up),Vector3.up)) * m_AirSteeringSpeed;
        //
        //    if (Vector3.Dot(MovementVector, transform.forward) > m_AirMinSpeed) //If the value is too high, manuevering doesnt happen
        //    {
        //        MovementVector.z = Mathf.Lerp(MovementVector.z, airInput.z, m_AirInertiaIntensity);
        //        MovementVector.x = Mathf.Lerp(MovementVector.x, airInput.x, m_AirInertiaIntensity);
        //    }
        //}
    }

    private void fadeShadow()
    {
        RaycastHit shadowHit;
        Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), transform.TransformDirection(Vector3.down), out shadowHit, Mathf.Infinity);
        m_dropShadow.m_FadeFactor = 1 - m_dropShadowFadeMultiplier * shadowHit.distance;
    }
    
    public void fallingBehavior()
    {
        MovementVector.y -= m_gravity * Time.fixedDeltaTime;
    }
    
}
