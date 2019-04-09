using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float CharacterSpeed = 20.0f;
    public float steeringSpeed = 1.0f;

    enum controlState//the animator should ultimately removethe need for this,I put it there just in case it is needed
    {
        walking,
        running,
        sprinting,
        airborne,
        falling
    }

    private void Awake()
    {
        controller = GetComponentInChildren<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        //transform.rotation = 



    }

    private void Jump()
    {
        Debug.Log("Jump!");
    }



}
