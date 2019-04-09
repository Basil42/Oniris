using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PH_Movement : MonoBehaviour
{

    //private int speed;
    private Rigidbody rb;

    public Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        //speed = 10;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        // Set some local float variables equal to the value of our Horizontal and Vertical Inputs
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float jump;
        if (Input.GetButton("Jump"))
        {
            jump = 0.1f;
        }
        else
        {
            jump = 0;
        }

        if (Input.GetKey(KeyCode.Z))
        {
            jump = -0.1f;
        }
        

        // Create a Vector3 variable, and assign X and Z to feature our horizontal and vertical float variables above
        //Vector3 movement = new Vector3(moveHorizontal, jump, moveVertical);

        // Add a physical force to our Player rigidbody using our 'movement' Vector3 above, 
        // multiplying it by 'speed' - our public player speed that appears in the inspector
        //rb.AddForce(movement * speed);
      //  transform.position = new Vector3(transform.position.x + moveHorizontal * 0.1f, transform.position.y + jump, transform.position.z + moveVertical * 0.1f);
       
        //Trying to make the player move in the direction the camera is facing, will delay until we have
        //a basic movement though, since it depends kind of heavily on that.

       // transform.position = new Vector3(transform.position.x + moveHorizontal * 0.1f, transform.position.y + jump, transform.position.z + moveVertical * 0.1f);
        transform.position = new Vector3(transform.position.x + Camera.main.transform.forward.x * moveVertical * 0.1f, transform.position.y + jump, transform.position.z + Camera.main.transform.forward.z * moveVertical * 0.1f);
    }
}
