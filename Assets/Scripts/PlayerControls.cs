using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class PlayerControls : MonoBehaviour
{
    //references
    private Transform m_Camera;
    private Vector3 m_CameraForward;
    private Vector3 m_lStickInputVector; //camera relative
    private PlayerMovement m_playerMove;
    //internal state
    private bool m_jumpPressed = false;
    private bool m_jumpHeld = false;
    private bool m_jumpTest = false;

    private float h_lstick;
    private float v_lstick;    


    // Start is called before the first frame update
    void Start()
    {
        m_Camera = Camera.main.transform;
        m_playerMove = GetComponentInChildren<PlayerMovement>();
        if (m_playerMove == null)
        {
            Debug.LogError("No valid player movement script found", gameObject);
        }



        Debug.Log("controls on");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            m_jumpPressed = true;
        }



        //adapted from some of the standard unity asset code
        h_lstick = Input.GetAxis("Horizontal");
        v_lstick = Input.GetAxis("Vertical");

        //camera relative input
        if (m_Camera != null)
        {
            m_CameraForward = Vector3.Scale(m_Camera.forward, new Vector3(1, 0, 1)).normalized;//1,0,1 is the ground plane

            m_lStickInputVector = h_lstick * m_Camera.right + v_lstick * m_CameraForward;
            //pass the input vector to the stuff that caresabout it
            m_playerMove.Jump(m_jumpPressed);
            m_playerMove.Move(m_lStickInputVector);

        }

        if (Input.GetButtonDown("Blink"))
        {
            m_playerMove.Blink(m_lStickInputVector);
        }

        m_jumpPressed = false;

    }

    

    private void FixedUpdate()
    {

    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        Debug.Log("Jump input received");
    }
}
