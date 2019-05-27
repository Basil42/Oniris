using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControls : MonoBehaviour
{
    //references
    private Transform m_Camera;
    private Vector3 m_CameraForward;
    private Vector3 m_CameraRight;
    private Vector3 m_lStickInputVector; //camera relative
    private PlayerMovement m_playerMove;
    private Blink m_blinkScript;
    private Dash m_dashScript;
    private Jump m_jumpScript;
    private WallJump m_wallJumpScript;
    private PauseMenu m_pauseMenuScript;
    //internal state




    // Start is called before the first frame update
    void Start()
    {
        m_Camera = Camera.main.transform;
        m_playerMove = GetComponentInChildren<PlayerMovement>();
        m_blinkScript = GetComponentInChildren<Blink>();
        m_dashScript = GetComponentInChildren<Dash>();
        m_jumpScript = GetComponentInChildren<Jump>();
        m_wallJumpScript = GetComponentInChildren<WallJump>();
        m_pauseMenuScript = GameObject.FindGameObjectWithTag("Canvas").GetComponent<PauseMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("WallRunToggle") > 0.5f)
        {
            m_playerMove.m_abilityFlags |= AbilityAvailability.WallJumpOn;
        }
        else
        {
            m_playerMove.m_abilityFlags &= ~AbilityAvailability.WallJumpOn;
        }
        if (Input.GetButtonDown("PauseMenu")) m_pauseMenuScript.PauseResume();
        if (m_pauseMenuScript.GameIsPaused) return;
        if (Input.GetButtonDown("Jump"))
        {
            switch (m_playerMove.m_state)
            {
                case movementState.grounded:
                case movementState.falling:
                case movementState.dashing:
                case movementState.offLedge:
                    m_jumpScript.jump();
                    break;
                case movementState.wallrunLeft:
                case movementState.wallrunRight:
                    m_wallJumpScript.WallJumpLateral();
                    break; 
                case movementState.wallrunFront:
                    m_wallJumpScript.WallJumpFront();
                    break;    
                default:
                    //jump buffering ?
                    break;
            }
            
        }
        if (Input.GetButtonUp("Jump"))
        {
            if (m_playerMove.m_state == movementState.jumping) m_jumpScript.stopJumping();
            else if (m_playerMove.m_state == movementState.doubleJumping) m_jumpScript.stopDoublejumping();
        }
        if (Input.GetButtonDown("Blink")&& m_playerMove.m_abilityFlags.HasFlag(AbilityAvailability.hasBlink) && m_playerMove.m_abilityFlags.HasFlag(AbilityAvailability.blink) && (m_playerMove.m_state == movementState.grounded || m_playerMove.m_state == movementState.jumping|| m_playerMove.m_state == movementState.falling|| m_playerMove.m_state == movementState.doubleJumping)) m_blinkScript.blink(m_lStickInputVector);
        //if (Input.GetButtonDown("Dash") && m_playerMove.m_abilityFlags.HasFlag(AbilityAvailability.hasDash) && m_playerMove.m_abilityFlags.HasFlag(AbilityAvailability.dash)) m_dashScript.dash();
    }

    private void FixedUpdate()
    {
        m_CameraForward = Vector3.Scale(m_Camera.forward, new Vector3(1, 0, 1)).normalized;//1,0,1 is the ground plane
        m_CameraRight = Vector3.Scale(m_Camera.right, new Vector3(1, 0, 1)).normalized;
        m_lStickInputVector = Input.GetAxis("Horizontal") * m_CameraRight + Input.GetAxis("Vertical") * m_CameraForward;

        m_playerMove.m_inputvector = m_lStickInputVector;
    }
}
