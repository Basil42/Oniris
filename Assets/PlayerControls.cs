using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    //references
    private Transform m_Camera;
    private Vector3 m_CameraForward;
    private Vector3 m_lStickInputVector; //camera relative
    private PlayerMovement m_playerMove;
    //internal state
    private bool m_jumpPressed =false;


    // Start is called before the first frame update
    void Start()
    {
        m_Camera = Camera.main.transform;
        m_playerMove = GetComponentInChildren<PlayerMovement>();
        if(m_playerMove == null)
        {
            Debug.LogError("No valid player movement script found",gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            m_jumpPressed = true;
        }
        //adapted from some of the standard unity asset code
        float h_lstick = Input.GetAxis("Horizontal");
        float v_lstick = Input.GetAxis("Vertical");

        //camera relative input
        if (m_Camera != null)
        {
            m_CameraForward = Vector3.Scale(m_Camera.forward, new Vector3(1, 0, 1)).normalized;//1,0,1 is the ground plane

            m_lStickInputVector = h_lstick * m_Camera.right + v_lstick * m_CameraForward;
            //pass the input vector to the stuff that caresabout it
            m_playerMove.Move(m_lStickInputVector);
            m_playerMove.Jump(m_jumpPressed);
        }

        m_jumpPressed = false;
    }

    private void FixedUpdate()
    {
      
    }
}
