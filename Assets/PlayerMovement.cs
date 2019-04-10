using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (CharacterController))]
public class PlayerMovement : MonoBehaviour 
{
    private CharacterController controller;
    public float m_RunningSpeed = 1.0f;//the walk animation runs if the input vector is small enough
    public float m_SteeringSpeed = 1.0f;
    public float m_gravity = 10.0f;
    private Vector3 MovementVector;

    

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
        if(buttonPress) Debug.Log("Jump!");
    }

    public void Move(Vector3 inputVector)
    {
        if (controller.isGrounded)
        {
            
            MovementVector = inputVector;
            MovementVector = MovementVector * m_RunningSpeed;
        }
        else
        {
            Debug.Log("controller not grounded");
            //put airborne behavior here
        }
        MovementVector.y = MovementVector.y - (m_gravity * Time.deltaTime);

        controller.Move(MovementVector);
    }

}
