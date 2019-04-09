using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    //references
    private CharacterController controller;
    private Transform m_Camera;
    private Vector3 m_CameraForward;
    private Vector3 m_lStickInputVector; //camera relative
    //internal state
    bool m_isJumping = false;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInChildren<CharacterController>();
        m_Camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isJumping)
        {
            //jump code in there to ensure no input is missed
        }
    }

    private void FixedUpdate()
    {
        //adapted from some of the standard unity asset code
        float h_lstick = Input.GetAxis("Horizontal");
        float v_lstick = Input.GetAxis("Vertical");

        //camera relative input
        if(m_Camera != null)
        {
            m_CameraForward = Vector3.Scale(m_Camera.forward, new Vector3(1, 0, 1)).normalized;
            m_lStickInputVector = h_lstick * m_Camera.right + v_lstick * m_CameraForward;

            //pass the input vector to the stuff that caresabout it
        }
    }
}
