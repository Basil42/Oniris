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

    //public int m_maxJumps = 2;
    //private int m_currentjumps = 0;
    private bool m_doubleJumped = false;
    private bool m_airSwitch = false;
    private bool m_grounded = false;

    private LayerMask blinkThrough;
    private Renderer[] blinkBodies;
    public float blinkDelay = 1;
    

    public Vector3 MovementVector;
    private Animator m_animator;

    private bool m_shortJump;
    private float m_jumpTransitionTimer = 0;
    public float m_jumpTransitionLimit = 0.3f; //Limit -> Length?

    

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        blinkThrough = ~ LayerMask.GetMask("Blinkable");
        blinkBodies = GetComponentsInChildren<Renderer>();
        Debug.Log("controller aquired");

    }
    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        //Debug.Log("speed " + MovementVector.y + "Gravity " + m_gravity);
        //Debug.Log("Gravity " );
    }

    private void LateUpdate()
    {
        
    }

    public void Blink(Vector3 inputVector)
    {
        controller.enabled = false;
        print("Don't Blink");
        RaycastHit hit;

        inputVector.y = 0;
        inputVector = inputVector.normalized;

        Vector3 distanceTravelled;
       
        //Velocity, magnitude of movement gets applied in input direction after blink
        Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), transform.TransformDirection(inputVector), out hit, 10, blinkThrough);

        if (hit.distance == 0)
        {
            transform.position = transform.position + inputVector * 10;
            distanceTravelled = inputVector * 10;
        } else
        {
            transform.position = transform.position + inputVector * (hit.distance - 0.5f);
            distanceTravelled = inputVector * (hit.distance - 0.5f);
            //print(hit.distance);
        }

        foreach(Renderer renderer in blinkBodies)
        {
            renderer.enabled = false;
        }

        float mag = MovementVector.magnitude;

        //Invoke("Appear", blinkDelay);
        //StartCoroutine(MoveCamera(distanceTravelled));
        StartCoroutine(Appear(inputVector, mag));
        
    }

    //private IEnumerator MoveCamera(Vector3 targetPosition)
    //{
    //    //Clock?

    //    Camera camera = Camera.main;

    //    targetPosition = camera.transform.position + targetPosition;

    //    camera.GetComponent<Cinemachine.CinemachineBrain>().enabled = false;
        
    //    while (camera.transform.position.x != targetPosition.x && camera.transform.position.z != targetPosition.z)
    //    {
    //        camera.transform.position = Vector3.Lerp(camera.transform.position, targetPosition, 0.2f);
    //        Debug.Log("Moving");
    //        yield return null;
    //    }
        
    //    camera.GetComponent<Cinemachine.CinemachineBrain>().enabled = true;
    //   //yield return null;
    //    //    //Lerp towards the new position, which is in the direction traveled by the blink.
    //}

    private IEnumerator Appear(Vector3 direction, float magnitude)
    {
        yield return new WaitForSeconds(blinkDelay);

        controller.enabled = true;

        foreach (Renderer renderer in blinkBodies)
        {
            renderer.enabled = true;
        }

        MovementVector.x = direction.x;
        MovementVector.z = direction.z;
        MovementVector.y = 0;

        MovementVector = MovementVector * magnitude;
        //Camera.main.GetComponent<Cinemachine.CinemachineBrain>().enabled = true;
    }

    public void Jump(bool buttonPress)
    {
        if (buttonPress && /*m_currentjumps < m_maxJumps*/ m_grounded)
        {
            MovementVector.y = m_jumpingSpeed;
            Debug.Log("Input value " + MovementVector.y);

            m_shortJump = false;
            m_jumpTransitionTimer = 0;
            //m_currentjumps++;
        } else if (buttonPress && !m_doubleJumped)
        {
            MovementVector.y = m_jumpingSpeed; // * m_RunningSpeed;
            Debug.Log("Double Jump " + MovementVector.y);

            m_doubleJumped = true;
            m_shortJump = false;
            m_jumpTransitionTimer = 0;
        }

    }

    public void Move(Vector3 inputVector)
    {

        if (m_grounded)
        {
            //Lerp toward input vector
            MovementVector.x = inputVector.x * m_RunningSpeed;

            MovementVector.z = inputVector.z * m_RunningSpeed;

            //MovementVector = MovementVector * m_RunningSpeed;

            m_doubleJumped = false;
            m_airSwitch = false;



            //MovementVector.y = MovementVector.y - (m_gravity * Time.deltaTime);
            if (MovementVector.magnitude > 0.05f) transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(new Vector3(MovementVector.x,0.0f,MovementVector.z), Vector3.up), m_SteeringSpeed);
        }
        else
        {


            //put airborne behavior here

            if(m_doubleJumped && !m_airSwitch)
            {
                m_airSwitch = true;

                MovementVector.x = inputVector.x * m_RunningSpeed;
                MovementVector.z = inputVector.z * m_RunningSpeed;
            }

            //Short jump behaviour

            // could do this another way: apply force if getbuttonDown, if not stop applying force

            if (m_jumpTransitionTimer < m_jumpTransitionLimit && Input.GetButtonUp("Jump"))
            {
                m_shortJump = true;
            }

            if (m_shortJump)
            {
                MovementVector.y = MovementVector.y - (m_gravity * 4 * Time.fixedDeltaTime);
                //Debug.Log("Movement " + MovementVector.y);
            }
            else
            {
                MovementVector.y = MovementVector.y - (m_gravity * Time.fixedDeltaTime);
                m_jumpTransitionTimer += Time.fixedDeltaTime;
                //Debug.Log("Movement " + MovementVector.y);
            }

            if (m_grounded)
            {
                m_shortJump = false;
                
            }
        }


        if (MovementVector.y < -1.0f && m_grounded)
        {
            
            MovementVector.y = -1.0f;//arbitrary value to keep it from growing ever bigger
           
            Debug.Log("Physics movement vector " + MovementVector.y);
        }

        controller.Move(MovementVector);
    }

    private void GroundCheck()
    {
        RaycastHit hit;
        RaycastHit hit2;
        
        
        if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), transform.TransformDirection(Vector3.down), out hit, 0.1f) |
            Physics.Raycast(transform.position + (transform.forward * 0.5f + transform.up * 0.1f), transform.TransformDirection(Vector3.down), out hit2, 0.3f)
            )
        {
            Debug.DrawRay(transform.position + new Vector3(0, 0.1f, 0), transform.TransformDirection(Vector3.down) * hit2.distance, Color.yellow);
            Debug.DrawRay(transform.position + (transform.forward * 0.5f + transform.up * 0.3f), transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
            Debug.Log("Did Hit");
            m_grounded = true;
        }
        else
        {
            Debug.DrawRay(transform.position + new Vector3(0, 0.1f, 0), transform.TransformDirection(Vector3.down) * 1000, Color.white);
            Debug.DrawRay(transform.position + (transform.forward * 0.2f + transform.up * 0.1f), transform.TransformDirection(Vector3.down) * 1000, Color.white);
            Debug.Log("Did not Hit");
            m_grounded = false;
        }
    }
}
