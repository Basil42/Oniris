using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public float speed = 1;
    private float originalSpeed;
    public float cooldown = 2;
    private float timer;
 
    public float decelerationDelay = 1;
    public float decelerationStep = 0.2f;
    private float decelerationAmount;
    private float targetVelocityX;
    private float targetVelocityZ;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        targetVelocityX = playerMovement.m_RunningSpeed;
        targetVelocityZ = targetVelocityX;
        originalSpeed = speed;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        //TODO: Dash charges
    }

    //Dash in direction of input
    public void dash()
    {
        if (timer >= cooldown)
        {
            Debug.Log("dash");
            speed = originalSpeed;
            playerMovement.m_state = movementState.dashing;
            playerMovement.MovementVector += speed * transform.forward;
            playerMovement.MovementVector.y = 0;
            timer = 0;

            playerMovement.m_abilityFlags ^= AbilityAvailability.dash;

            transform.rotation = Quaternion.LookRotation(new Vector3(playerMovement.MovementVector.x, 0.0f, playerMovement.MovementVector.z));

            //coroutine function to reduce speed after having dashed
            
            StartCoroutine("decelerate");
        }
    }

    private IEnumerator decelerate()
    {
        yield return new WaitForSeconds(decelerationDelay);
        
        int x = 0;
        float velocity;
        decelerationAmount = 0;
        Vector3 HorizontalMovementVector = new Vector3(playerMovement.MovementVector.x, 0.0f, playerMovement.MovementVector.z);
        while (x <= 250 && ( HorizontalMovementVector.magnitude > playerMovement.m_RunningSpeed) && playerMovement.m_state == movementState.falling) 
        {
            //Lerp towards a target velocity
            decelerationAmount += decelerationStep;
            velocity = Mathf.Lerp(HorizontalMovementVector.magnitude, playerMovement.m_RunningSpeed, decelerationAmount);//playerMovement.MovementVector.magnitude
            HorizontalMovementVector = HorizontalMovementVector.normalized * velocity;
            playerMovement.MovementVector = new Vector3(HorizontalMovementVector.x, playerMovement.MovementVector.y, HorizontalMovementVector.z);
            yield return new WaitForFixedUpdate();

            //Alternative deceleration
            //speed *= 0.995f;
            //playerMovement.MovementVector = playerMovement.MovementVector - (originalSpeed * transform.forward) + (speed * transform.forward);
            //HorizontalMovementVector = new Vector3(playerMovement.MovementVector.x, 0.0f, playerMovement.MovementVector.z);
            x++;
            print("decelerating");
        }
        playerMovement.GroundCheck();
        print("decelerated");
    }
}
