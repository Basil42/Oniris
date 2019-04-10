using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour 
{
    private CharacterController controller;
    public float m_RunningSpeed = 1.0f;//the walk animation runs if the input vector is small enough
    public float m_SteeringSpeed = 1.0f;
    public float m_gravity = 10.0f;
    public float m_jumpingSpeed = 10.0f;

    private Vector3 MovementVector;
    private Animator m_animator;

    

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
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
            Debug.Log(buttonPress);
        }
    }

    public void Move(Vector3 inputVector)
    {
        if (controller.isGrounded)
        {
            
            MovementVector.x = inputVector.x;
            MovementVector.z = inputVector.z;
            MovementVector = MovementVector * m_RunningSpeed;

            
            if (MovementVector.magnitude > 0.05f) transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(new Vector3(MovementVector.x,0.0f,MovementVector.z), Vector3.up),0.2f);
        }
        else
        {
            Debug.Log("controller not grounded");
            
            //put airborne behavior here
        }
        if (controller.isGrounded)
        {
            MovementVector.y = MovementVector.y - (m_gravity * Time.deltaTime);
        }else if(MovementVector.y < -1.0f)
        {
            MovementVector.y = -1.0f;
        }
        controller.Move(MovementVector);
    }

}
