using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Blink : MonoBehaviour
{
    PlayerMovement PlayerMovement;

    private CameraController cameraController;

    private LayerMask blinkThrough;
    private Renderer[] blinkBodies;
    public float distance = 10;
    public float blinkStep = 50;

    public float blinkDuration = 10;
    private float timer = 0.0f;

    public bool m_BlinkEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        blinkThrough = ~LayerMask.GetMask("Blinkable");// "everything but blinkable
        blinkBodies = GetComponentsInChildren<Renderer>();
        PlayerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    
    public void blink(Vector3 inputVector)
    {
        PlayerMovement.controller.enabled = false;
        PlayerMovement.m_state = movementState.blinking;
        print("Don't Blink");

        inputVector.y = 0;
        inputVector = inputVector.normalized;//shouldn't be here

        foreach (Renderer renderer in blinkBodies)
        {
            renderer.enabled = false;
        }

        //Velocity, magnitude of movement gets applied in input direction after blink
        RaycastHit hit;
        Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0),inputVector, out hit, distance, blinkThrough);

        if (hit.distance == 0)
        {
            StartCoroutine(Blinking(inputVector ,transform.position + inputVector * distance));  
        }
        else
        {
            StartCoroutine(Blinking(inputVector ,transform.position + inputVector * (hit.distance - 0.5f)));
            print(hit.distance);
        }
    }

    private IEnumerator Blinking(Vector3 inputVector, Vector3 targetPosition)
    {
        //Deciding momentum upon leaving the blink
        float mag = PlayerMovement.MovementVector.magnitude;
        PlayerMovement.MovementVector.x = inputVector.x;
        PlayerMovement.MovementVector.z = inputVector.z;
        PlayerMovement.MovementVector.y = 0;
        PlayerMovement.MovementVector = PlayerMovement.MovementVector * mag;

        //Blinking
        timer = 0.0f;
        while(timer <= blinkDuration)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, blinkStep * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
        }

        foreach (Renderer renderer in blinkBodies)
        {
            renderer.enabled = true;
        }

        PlayerMovement.controller.enabled = true;
        PlayerMovement.m_state = movementState.falling;
        print("Finished blinking");
    }
}
