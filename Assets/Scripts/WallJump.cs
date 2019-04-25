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
    [Tooltip("Distance beyond the player's collider the game will 'feel' for walls")]
    [SerializeField] private float m_reach = 0.5f;
    private float m_absoluteReach;

    //allocations
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
        //check for falling/jumping state
        if (detectWalls())
        {
            InitiateWallRun();
        }
        else
        {
            Debug.Log(m_shortestHitDistance);
        }

    }

    private bool detectWalls()
    {
        m_shortestHitDistance = m_absoluteReach + 1.0f;//initialized here so any hit will be shorter
        m_direction = Direction.None;
        m_origin = transform.position + new Vector3(0.0f, m_controlller.height / 2.0f, 0.0f);
        CheckDirection(new Ray(m_origin, transform.forward), Direction.Front);
        CheckDirection(new Ray(m_origin, transform.right), Direction.Right);
        CheckDirection(new Ray(m_origin, -transform.right), Direction.Left);
        
        return (m_direction != Direction.None);
    }

    private void CheckDirection(Ray ray, Direction direction)
    {
        if (Physics.Raycast(ray, out m_hit, m_absoluteReach) && m_hit.distance < m_shortestHitDistance)
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
                Debug.Log("wall in front");
                break;
            case Direction.Left:
                Debug.Log("Wall on the left");
                break;
            case Direction.Right:
                Debug.Log("Wall on the right");
                break;
            default:
                break;
        }
    }
}
