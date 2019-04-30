using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for jumping and double jumping
public class Jump : MonoBehaviour
{

    [SerializeField] private float m_jumpingSpeed = 2.0f;
    //[SerializeField] private float m_DoubleJumpSpeed = 2.0f;//should matter, implement once state maching is in place

    private PlayerMovement playerMovement;

    private float m_jumpTimer;
    public float m_jumpLength = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerMovement.m_state == movementState.jumping) JumpBehavior();
        else if (playerMovement.m_state == movementState.doubleJumping) DoubleJumpBehavior();
    }

    public void jump()
    {
        
        if (playerMovement.m_state == movementState.grounded || playerMovement.m_state == movementState.offLedge)
        {
            print("Jumping");
            playerMovement.m_state = movementState.jumping;
            m_jumpTimer = 0; 
        }
        else if (playerMovement.m_abilityFlags.HasFlag(AbilityAvailability.doubleJump) && playerMovement.m_state == movementState.falling) 
        {
            print("DoubleJump");
            
            playerMovement.MovementVector.x = playerMovement.m_inputvector.x * playerMovement.m_RunningSpeed;
            playerMovement.MovementVector.z = playerMovement.m_inputvector.z * playerMovement.m_RunningSpeed;
            transform.rotation = Quaternion.LookRotation(new Vector3(playerMovement.MovementVector.x,0.0f,playerMovement.MovementVector.z), Vector3.up);
            playerMovement.m_state = movementState.doubleJumping;
            playerMovement.m_abilityFlags &= ~AbilityAvailability.doubleJump;//set the double jump to unavailable
            m_jumpTimer = 0;
        }
    }

    public void stopJumping()
    {
        print("stopJumping");
       
        playerMovement.m_state = movementState.falling;
        if (m_jumpTimer < m_jumpLength) //If the jump has not reached maximum height already, reduce velocity 
        {                               //to increase responsiveness
            playerMovement.MovementVector.y = playerMovement.MovementVector.y / 2;
        }
    }

    public void stopDoublejumping()
    {
        playerMovement.m_state = movementState.falling;
        if (m_jumpTimer < m_jumpLength) //If the jump has not reached maximum height already, reduce velocity 
        {                               //to increase responsiveness
            playerMovement.MovementVector.y = playerMovement.MovementVector.y / 2;
        }

    }

   

    //Run this while jumping
    public void JumpBehavior()
    {
        if (m_jumpTimer < m_jumpLength)
        {
            playerMovement.MovementVector.y = m_jumpingSpeed;
            m_jumpTimer += Time.deltaTime;
        }
        else 
        {
            playerMovement.m_state = movementState.falling;
            
        }
    }
    private void DoubleJumpBehavior()
    {
        if (m_jumpTimer < m_jumpLength)
        {
            playerMovement.MovementVector.y = m_jumpingSpeed;
            m_jumpTimer += Time.deltaTime;
        }
        else
        {
            playerMovement.m_state = movementState.falling;

        }
    }
}
