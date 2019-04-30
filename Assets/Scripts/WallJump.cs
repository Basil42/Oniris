using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(CharacterController))]
public class WallJump : MonoBehaviour
{
    
    PlayerMovement m_movementScript;
    CharacterController m_controlller;
    [Tooltip("Distance beyond the player's collider the game will 'feel' for walls")]
    [SerializeField] private float m_reach = 0.5f;
    [Tooltip("tolerance in slope for what checks out as a wall (higher values make requirement less strigent")]
    [SerializeField] private float m_slopeTreshold = 0.1f;//this value determines how much leeway the game gives something to be considered a wall (colliders can have a bit of slope and still by "walljumpable")
    [Tooltip("Speed at which the character wall run vertically")]
    [SerializeField] private float wallClimbSpeed = 0.3f;
    [Tooltip("Speed at which the player bounces off the wall if they did not jump off of it before the wall run ends")]
    [SerializeField] private float bounceVelocity = 1.0f;

    private float m_absoluteReach;

    //allocations for values that need regular updating (blackboard)
    private float m_shortestHitDistance;//used in choosing which ray determines the angle of collision with walls
    private RaycastHit m_hit;//allocated memory to read raycast hits
    private Direction m_direction;
    private Vector3 m_origin;  //origin of the rays that detect walls
    
    enum Direction
    {
        None,
        Front,
        Left,
        Right,
    }

    private void Awake()
    {
        m_movementScript = GetComponent<PlayerMovement>();
        m_controlller = GetComponent<CharacterController>();
        m_absoluteReach = m_controlller.radius + m_controlller.skinWidth + m_reach;

    }

    private void FixedUpdate()
    {
        //check for falling/jumping state check for minimal speed
        m_origin = transform.position + new Vector3(0.0f, m_controlller.height / 2.0f, 0.0f);
        switch (m_movementScript.m_state)
        {
            case movementState.grounded:
                break;
            case movementState.falling:
            case movementState.doubleJumping:
            case movementState.offLedge:
            case movementState.jumping:
                if (detectWalls()) InitiateWallRun();
                break;

            case movementState.wallrunLeft:
                wallRunLeftBehavior();
                break;
            case movementState.wallrunRight:
                wallRunRightBehavior();
                break;
            case movementState.wallrunFront:
                wallRunFrontBehavior();
                break;
            default:
                break;
        }



    }

    private void wallRunFrontBehavior()
    {
        
        if (!Physics.Raycast(m_origin, transform.forward, out m_hit))
        {
            m_movementScript.MovementVector = -transform.forward * bounceVelocity;
            m_movementScript.m_state = movementState.falling;
            transform.forward = -transform.forward;
            Debug.Log("bounce");
        }
    }

    private void wallRunRightBehavior()
    {
        if (!Physics.Raycast(m_origin, transform.right, out m_hit))
        {
            m_movementScript.MovementVector += m_hit.normal * bounceVelocity;
            m_movementScript.m_state = movementState.falling;
        }
    }

    private void wallRunLeftBehavior()
    {
        if (!Physics.Raycast(m_origin, -transform.right, out m_hit))
        {
            m_movementScript.MovementVector += m_hit.normal * bounceVelocity;
            m_movementScript.m_state = movementState.falling;
        }
    }

    private bool detectWalls()
    {
        m_shortestHitDistance = m_absoluteReach + 1.0f;//initialized here so any hit will be shorter
        m_direction = Direction.None;
        
        CheckDirection(new Ray(m_origin, transform.forward), Direction.Front);
        CheckDirection(new Ray(m_origin, transform.right), Direction.Right);
        CheckDirection(new Ray(m_origin, -transform.right), Direction.Left);
        
        return (m_direction != Direction.None);
    }

    private void CheckDirection(Ray ray, Direction direction)
    {
        if (Physics.Raycast(ray, out m_hit, m_absoluteReach) && m_hit.distance < m_shortestHitDistance && Vector3.Dot(m_hit.normal, m_movementScript.MovementVector) < 0  && Mathf.Abs(m_hit.normal.y)< m_slopeTreshold)
        {
            m_shortestHitDistance = m_hit.distance;
            m_direction = direction;
        }
    }
    

    private void InitiateWallRun()
    {
        switch (m_direction)
        {
            case Direction.None:
                Debug.LogError("Invalid walljump direction, value has been modified at an unexpected point");
                break;
            case Direction.Front:
                m_movementScript.m_state = movementState.wallrunFront;
                m_movementScript.MovementVector = Vector3.up * wallClimbSpeed;
                transform.rotation = Quaternion.LookRotation(-m_hit.normal,Vector3.up);//line upthe character properly
                Debug.Log("wallrun forward");
                break;
            case Direction.Left:
                m_movementScript.m_state = movementState.wallrunLeft;

                break;
            case Direction.Right:
                m_movementScript.m_state = movementState.wallrunRight;
                break;
            default:
                break;
        }
    }
}
