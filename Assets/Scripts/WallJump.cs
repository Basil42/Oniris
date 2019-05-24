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
    [Tooltip("tolerance in slope for what checks out as a wall (higher values make requirement less strigent, 1 makes horizontal surface valid)")]
    [SerializeField] private float m_slopeTreshold = 0.2f;//this value determines how much leeway the game gives something to be considered a wall (colliders can have a bit of slope and still by "walljumpable")
    [Tooltip("Speed at which the character wall run vertically")]
    [SerializeField] private float wallClimbSpeed = 0.3f;
    [Tooltip("Speed at which the character wall run horizontally")]
    [SerializeField] private float m_wallrunSpeed = 0.3f;
    [Tooltip("Speed at which the player bounces off the wall if they did not jump off of it before the wall run ends")]
    [SerializeField] private float bounceVelocityLateral = 1.0f;
    [Tooltip("Speed at which the player bounces off the wall if they did not jump off of it before the wall run ends")]
    [SerializeField] private float bounceVelocityVertical = 1.0f;
    [Tooltip("Angle at which the player starts their horizontal wall run 90 is purely horizontal")]
    [SerializeField] private float m_RunAngle = 90.0f;
    [Tooltip("Time in seconds the player can 'stick' to a wall while wall running horizontally")]
    [SerializeField] private float m_LateralRunTime = 3.0f;
    [Tooltip("Time in seconds the player can 'stick' to a wall while wall running vertically")]
    [SerializeField] private float m_VerticalRunTime = 1.5f;
    [SerializeField] private float verticalWallJumpHeight = 0.5f;
    [SerializeField] private float verticalWallJumpLength = 0.2f;
    [SerializeField] private float m_verticalSurfaceSizeTreshold = 0.1f;
    [SerializeField] private float m_lateralWallJumpHeight = 0.5f;
    [SerializeField] private float m_lateralWallJumpLength = 0.2f;
    [SerializeField] private float m_LateralSurfaceSizeTreshold = 0.1f;
    private float m_absoluteReach;

    //allocations for values that need regular updating (blackboard)
    private float m_shortestHitDistance;//used in choosing which ray determines the angle of collision with walls
    private RaycastHit m_hit;//allocated memory to read raycast hits
    private RaycastHit m_chosenHit;
    private Vector3 m_previousNormal;//last wall grabbed
    private RaycastHit m_securityhit;
    private Direction m_direction;
    private Vector3 m_origin;  //origin of the rays that detect walls
    private float m_RunTimer;
    [SerializeField]private float m_LateralJumpAngle = 0.5f;

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
        if (m_movementScript.m_abilityFlags.HasFlag(AbilityAvailability.hasWallJump))
        {
            m_origin = transform.position + new Vector3(0.0f, m_controlller.height / 2.0f, 0.0f);
            switch (m_movementScript.m_state)
            {
                case movementState.grounded:
                    m_previousNormal = Vector3.zero;
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
    }
    private void wallRunFrontBehavior()
    {
        m_RunTimer -= Time.fixedDeltaTime;
        if (!Physics.Raycast(m_origin, transform.forward, out m_hit, m_absoluteReach) || m_RunTimer < 0 || m_hit.normal != m_chosenHit.normal || !m_movementScript.m_abilityFlags.HasFlag(AbilityAvailability.WallJumpOn))//to do timer
        {
           
            
            m_movementScript.MovementVector = m_chosenHit.normal * bounceVelocityVertical;
            m_movementScript.m_state = movementState.falling;
            m_movementScript.m_animator.SetTrigger("fall");
            transform.forward = -transform.forward;
            Debug.Log("bounce");
        }
        
    }
    public void WallJumpFront()
    {
        //animation stuff 
        m_movementScript.MovementVector = Vector3.up * verticalWallJumpHeight + m_chosenHit.normal * verticalWallJumpLength;
        transform.forward = new Vector3(m_movementScript.MovementVector.x, 0.0f, m_movementScript.MovementVector.z).normalized;
        m_movementScript.m_state = movementState.falling;
        m_movementScript.m_animator.SetTrigger("jump");
    

    }
    private void wallRunRightBehavior()
    {
        m_RunTimer -= Time.fixedDeltaTime;
        if (!Physics.Raycast(m_origin, transform.right, out m_hit, m_absoluteReach) || m_RunTimer < 0 || m_hit.normal != m_chosenHit.normal || !m_movementScript.m_abilityFlags.HasFlag(AbilityAvailability.WallJumpOn))//to do : timer
        {
            m_movementScript.MovementVector = Vector3.Project(m_movementScript.MovementVector, transform.forward);
            m_movementScript.MovementVector += m_chosenHit.normal * bounceVelocityLateral;
            m_movementScript.m_state = movementState.falling;
            m_movementScript.m_animator.SetTrigger("fall");
            transform.forward = new Vector3(m_movementScript.MovementVector.x, 0.0f, m_movementScript.MovementVector.z).normalized;
            //facethe character in the appropriate direction(this function might not be responsible for it)
            Debug.Log("bounce");
        }
    }
    private void wallRunLeftBehavior()
    {
        m_RunTimer -= Time.fixedDeltaTime;
        if (!Physics.Raycast(m_origin, -transform.right, out m_hit,m_absoluteReach) || m_RunTimer < 0 || m_hit.normal != m_chosenHit.normal || !m_movementScript.m_abilityFlags.HasFlag(AbilityAvailability.WallJumpOn))//timer
        {
            m_movementScript.MovementVector = Vector3.Project(m_movementScript.MovementVector, transform.forward);
            m_movementScript.MovementVector += m_chosenHit.normal * bounceVelocityLateral;//bounce

            m_movementScript.m_state = movementState.falling;//might implement a custom walljump state and behavior later
            m_movementScript.m_animator.SetTrigger("fall");
            transform.forward = new Vector3(m_movementScript.MovementVector.x, 0.0f, m_movementScript.MovementVector.z).normalized;
            Debug.Log("bounce");
        }
    }
    public void WallJumpLateral()
    {
       
        m_movementScript.MovementVector = Vector3.up * m_lateralWallJumpHeight + Vector3.Slerp(transform.forward,m_chosenHit.normal, m_LateralJumpAngle) * m_lateralWallJumpLength;
        transform.forward = new Vector3(m_movementScript.MovementVector.x, 0.0f, m_movementScript.MovementVector.z).normalized;
        m_movementScript.m_state = movementState.falling;
        m_movementScript.m_animator.SetTrigger("jump");
    
    }
    private bool detectWalls()
    {
        if (!m_movementScript.m_abilityFlags.HasFlag(AbilityAvailability.WallJumpOn)) return false;
        m_shortestHitDistance = m_absoluteReach + 1.0f;//initialized here so any hit will be shorter
        m_direction = Direction.None;
        
        CheckDirection(new Ray(m_origin, transform.forward), Direction.Front);
        CheckDirection(new Ray(m_origin, transform.right), Direction.Right);
        CheckDirection(new Ray(m_origin, -transform.right), Direction.Left);
        
        return (m_direction != Direction.None);
    }
    private void CheckDirection(Ray ray, Direction direction)
    {
        if (Physics.Raycast(ray, out m_hit, m_absoluteReach) && 
            m_hit.distance < m_shortestHitDistance && 
            Vector3.Dot(m_hit.normal, m_movementScript.MovementVector) < 0  && 
            Mathf.Abs(m_hit.normal.y)< m_slopeTreshold && 
            Physics.Raycast(ray.origin + ((direction == Direction.Front)? Vector3.up * m_verticalSurfaceSizeTreshold: transform.forward * m_LateralSurfaceSizeTreshold),ray.direction, out m_securityhit, m_absoluteReach)&&
            m_securityhit.normal == m_hit.normal &&
            m_hit.normal != m_previousNormal)
        {
            m_shortestHitDistance = m_hit.distance;
            m_direction = direction;
            m_chosenHit = m_hit;
            
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
                m_RunTimer = m_VerticalRunTime;
                m_movementScript.m_state = movementState.wallrunFront;
                m_movementScript.m_animator.SetTrigger("wallrunFront");
                m_movementScript.MovementVector = Vector3.up * wallClimbSpeed;
                
                transform.rotation = Quaternion.LookRotation(-m_chosenHit.normal,Vector3.up);//line up the character properly
                

                //animation stuff
                break;
            case Direction.Left:
                m_RunTimer = m_LateralRunTime;
                m_movementScript.m_state = movementState.wallrunLeft;
                m_movementScript.m_animator.SetTrigger("wallrunlateral");
                m_movementScript.MovementVector = (Quaternion.AngleAxis(m_RunAngle,m_chosenHit.normal) * Vector3.up)* m_wallrunSpeed;

                //to do: line up character properly
                //to do : plane running
                break;
            case Direction.Right:
                m_RunTimer = m_LateralRunTime;
                m_movementScript.m_state = movementState.wallrunRight;
                m_movementScript.m_animator.SetTrigger("wallrunlateral");
                m_movementScript.MovementVector = (Quaternion.AngleAxis(-m_RunAngle, m_chosenHit.normal) * Vector3.up) * m_wallrunSpeed;

                break;
            default:
                break;
        }
        //to do : snap to the wall
        m_movementScript.MovementVector += -m_chosenHit.normal;
        m_previousNormal = m_chosenHit.normal;
    }
    public void Eject()
    {
        m_RunTimer = -1;
    }
}
