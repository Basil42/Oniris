using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour 
{
    private CharacterController controller;
    public float m_RunningSpeed = 1.0f;//the walk animation runs if the input vector is small enough
    public float m_SteeringSpeed = 0.2f;
    public float m_gravity = 10.0f;
    public float m_lowGravity = 5.0f;
    public float m_jumpingSpeed = 10.0f;
    

    private Vector3 MovementVector;
    private Animator m_animator;

    private bool m_shortJump;
    private float m_jumpTransitionTimer = 0;
    public float m_jumpTransitionLimit = 0.3f; //Limit -> Length?

    

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Debug.Log("controller aquired");
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        

        



    }

    public void Jump(bool buttonPress)
    {
        if (buttonPress && controller.isGrounded)
        {
            MovementVector.y = m_jumpingSpeed;
            Debug.Log("Input value " + MovementVector.y);

            m_shortJump = false;
            m_jumpTransitionTimer = 0;
        }

    }

    public void Move(Vector3 inputVector)
    {

        RaycastHit hit;
        // Does the ray intersect any objects?
        if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), transform.TransformDirection(Vector3.down), out hit, 1))
        {
            Debug.DrawRay(transform.position + new Vector3(0, 0.1f, 0), transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position + new Vector3(0, 0.1f, 0), transform.TransformDirection(Vector3.down) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }



        if (controller.isGrounded && Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), transform.TransformDirection(Vector3.down), out hit, 1))
        {
            //Lerp toward input vector
            MovementVector.x = inputVector.x;
            MovementVector.z = inputVector.z;
            MovementVector = MovementVector * m_RunningSpeed;

            //MovementVector.y = MovementVector.y - (m_gravity * Time.deltaTime);
            if (MovementVector.magnitude > 0.05f) transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(new Vector3(MovementVector.x,0.0f,MovementVector.z), Vector3.up), m_SteeringSpeed);
        }
        else
        {


            //put airborne behavior here


            //Short jump behaviour

            // could do this another way: apply force if getbuttonDown, if not stop applying force

            if (m_jumpTransitionTimer < m_jumpTransitionLimit && Input.GetButtonUp("Jump"))
            {
                m_shortJump = true;
            }

            if (m_shortJump)
            {
                MovementVector.y = MovementVector.y - (m_gravity * 4 * Time.deltaTime);
            }
            else
            {
                MovementVector.y = MovementVector.y - (m_gravity * Time.deltaTime);
                m_jumpTransitionTimer += Time.deltaTime;
            }

            if (controller.isGrounded)
            {
                m_shortJump = false;
            }
        }
        //if (controller.isGrounded)
        //{
        //    MovementVector.y = MovementVector.y - (m_gravity * Time.deltaTime);
        //}else 
        if (MovementVector.y < -1.0f)
        {
            
            MovementVector.y = -1.0f;//arbitrary value to keep it from growing ever bigger
           
            Debug.Log("Physics movement vector " + MovementVector.y);
        }

        controller.Move(MovementVector);
    }
}
