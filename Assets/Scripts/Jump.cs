using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{

    [SerializeField] private float m_jumpingSpeed = 2.0f;
    [SerializeField] private float m_DoubleJumpSpeed = 2.0f;

    private PlayerMovement PlayerMovement;

    private bool m_doubleJumped = false;
    private bool m_airSwitch = false;

    public float m_gravity = 0.3f;
    //public float m_lowGravity = 5.0f;


    private bool jumping = false;
    private float m_jumpTimer;
    public float m_jumpLength = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AirBehavior();
        if (PlayerMovement.m_grounded)
        {
            m_doubleJumped = false;
        }
    }

    public void jump()
    {
        
        if (PlayerMovement.m_grounded)
        {
            print("Jumping");
            jumping = true;
            m_jumpTimer = 0; 
        }
        else if (!m_doubleJumped) 
        {
            print("DoubleJump");
            PlayerMovement.airSwitch();
            jumping = true;
            m_doubleJumped = true;
            m_jumpTimer = 0;
        }
    }

    public void stopJumping()
    {
        print("stopJumping");
        jumping = false;
    }

    //TODO: momentum does not carry over between doublejumps, might want to fix

    //Run this while in the air
    public void AirBehavior()
    {
        if (jumping && m_jumpTimer < m_jumpLength)
        {
            PlayerMovement.MovementVector.y = m_jumpingSpeed;
            m_jumpTimer += Time.deltaTime;
        }
        else if (!PlayerMovement.m_grounded)
        {
            PlayerMovement.MovementVector.y = PlayerMovement.MovementVector.y - (m_gravity * Time.fixedDeltaTime);
        }
    }
}
