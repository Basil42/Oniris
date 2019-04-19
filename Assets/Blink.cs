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
    public float blinkDelay = 1;

    public bool m_BlinkEnabled = true;
    // Start is called before the first frame update
    void Start()
    {
        blinkThrough = ~LayerMask.GetMask("Blinkable");// "everything but blinkable
        blinkBodies = GetComponentsInChildren<Renderer>();
        PlayerMovement = GetComponent<PlayerMovement>();
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    // Update is called once per frame
    
    public void blink(Vector3 inputVector)
    {
        cameraController.CamSwitchBlink();
        PlayerMovement.controller.enabled = false;
        RaycastHit hit;

        inputVector.y = 0;
        inputVector = inputVector.normalized;


        //Velocity, magnitude of movement gets applied in input direction after blink
        Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), inputVector, out hit, 10, blinkThrough);

        if (hit.distance == 0)
        {
            transform.position = transform.position + inputVector * 10;
            Debug.Log(inputVector);
        }
        else
        {
            transform.position = transform.position + inputVector * (hit.distance - 0.5f);
            print(hit.distance);
        }

        foreach (Renderer renderer in blinkBodies)
        {
            renderer.enabled = false;
        }

        float Momentum = PlayerMovement.MovementVector.magnitude;


        StartCoroutine(Appear(inputVector, Momentum));

    }

    private IEnumerator Appear(Vector3 direction, float magnitude)
    {
        yield return new WaitForSeconds(blinkDelay);

        PlayerMovement.controller.enabled = true;

        foreach (Renderer renderer in blinkBodies)
        {
            renderer.enabled = true;
        }

        PlayerMovement.MovementVector.x = direction.x;
        PlayerMovement.MovementVector.z = direction.z;
        PlayerMovement.MovementVector.y = 0;

        PlayerMovement.MovementVector = PlayerMovement.MovementVector * magnitude;
    }
}
