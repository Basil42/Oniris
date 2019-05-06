using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Blink : MonoBehaviour
{
    PlayerMovement m_playerMovement;

    private CameraController cameraController;
    private CharacterController charCtrl;

    private LayerMask blinkThrough;
    private Renderer[] blinkBodies;
    public float distance = 10;
    public float blinkStep = 50;

    public float blinkDuration = 10;
    private float timer = 0.0f;

    public bool m_BlinkEnabled = true;
    private Vector3 blinkDirection;

    // Start is called before the first frame update
    void Start()
    {
        blinkThrough = ~LayerMask.GetMask("Blinkable");// "everything but blinkable
        blinkBodies = GetComponentsInChildren<Renderer>();
        m_playerMovement = GetComponent<PlayerMovement>();
        charCtrl = GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame
    
    public void blink(Vector3 inputVector)
    {
        m_playerMovement.controller.enabled = false;
        m_playerMovement.m_state = movementState.blinking;
        m_playerMovement.m_animator.SetTrigger("blink");
        print("Don't Blink");

        inputVector.y = 0;
        inputVector = inputVector.normalized;//shouldn't be here

        foreach (Renderer renderer in blinkBodies)
        {
            renderer.enabled = false;
        }

        //Velocity, magnitude of movement gets applied in input direction after blink
        RaycastHit hit;
        RaycastHit hit2;

        if (m_playerMovement.m_state == movementState.grounded)
        {
            print("grounded Blink");
            Physics.Raycast(transform.position + charCtrl.center, Vector3.down, out hit2, distance);
            blinkDirection = Vector3.ProjectOnPlane(inputVector, hit2.normal).normalized;
        }

        //spherecast starts from center of player, slightly behind them in the opposite direction of the inputVector
        Vector3 p1 = transform.position + charCtrl.center - blinkDirection;
        Physics.SphereCast(p1, charCtrl.height / 2, blinkDirection, out hit, distance, blinkThrough);

        m_playerMovement.m_state = movementState.blinking;

        if (hit.distance == 0)
        {
            //Raycast to check for blinkable colliders. 
            RaycastHit hit3;
            Physics.Raycast(transform.position + new Vector3(0, charCtrl.height / 2, 0), blinkDirection, out hit3, distance);
            
            //Checking whether there is a blinkable collider, and if the target blink position is inside it.
            if (hit3.collider == true && hit3.collider.gameObject.layer == 8 && hit3.collider.bounds.Contains(transform.position + inputVector * distance))
            {
                //If the target position is inside the collider, use the raycast that checks for blinkables as distance instead
                StartCoroutine(Blinking(inputVector, transform.position + blinkDirection * (hit3.distance - 0.5f)));
            }
            //No blinkable collider was found, or the target position was beyond it. Continue as normal, blink full distance
            else StartCoroutine(Blinking(inputVector, transform.position + blinkDirection * distance));
        }
        else
        {
            //Found a non-blinkable collider, position uses the hit.distance minus a small offset
            StartCoroutine(Blinking(inputVector ,transform.position + blinkDirection * (hit.distance - 0.5f)));
            print(hit.distance);
        }
    }

    private IEnumerator Blinking(Vector3 inputVector, Vector3 targetPosition)
    {
        //Deciding momentum upon leaving the blink
        float mag = m_playerMovement.MovementVector.magnitude;
        m_playerMovement.MovementVector.x = inputVector.x;
        m_playerMovement.MovementVector.z = inputVector.z;
        m_playerMovement.MovementVector.y = 0;
        m_playerMovement.MovementVector = m_playerMovement.MovementVector * mag;

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

        m_playerMovement.controller.enabled = true;
        m_playerMovement.GroundCheck();
        m_playerMovement.m_animator.SetTrigger((m_playerMovement.m_state == movementState.grounded ? "run" : "fall"));
        print("Finished blinking");
    }
}
