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
    //internal state
    
    private bool m_jumpHeld = false;
    

  

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = Camera.main.transform;
        m_playerMove = GetComponentInChildren<PlayerMovement>();
        m_blinkScript = GetComponentInChildren<Blink>();
    }

    // Update is called once per frame
    void Update()
    {
        //camera realative input
       

        if (Input.GetButtonDown("Jump")) m_playerMove.Jump();
        if (Input.GetButtonDown("Blink")) m_blinkScript.blink(m_lStickInputVector);//TO DO: put it in separate script
    }

    private void FixedUpdate()
    {
        m_CameraForward = Vector3.Scale(m_Camera.forward, new Vector3(1, 0, 1)).normalized;//1,0,1 is the ground plane
        m_CameraRight = Vector3.Scale(m_Camera.right, new Vector3(1, 0, 1)).normalized;
        m_lStickInputVector = Input.GetAxis("Horizontal") * m_Camera.right + Input.GetAxis("Vertical") * m_CameraForward;

        m_playerMove.Move(m_lStickInputVector);
    }
}
