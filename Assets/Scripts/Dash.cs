using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public float speed = 1;
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
            playerMovement.m_state = movementState.dashing;
            playerMovement.MovementVector += speed * transform.forward;
            playerMovement.MovementVector.y = 0;
            timer = 0;

            transform.rotation = Quaternion.LookRotation(new Vector3(playerMovement.MovementVector.x, 0.0f, playerMovement.MovementVector.z));

            //coroutine function to reduce speed after having dashed
            
            StartCoroutine("decelerate");
        }
    }

    private IEnumerator decelerate()
    {
        
        
        yield return new WaitForSeconds(decelerationDelay);
        playerMovement.GroundCheck();
        int x = 0;
        float velocity;
        decelerationAmount = 0;
        Vector3 HorizontalMovementVector = new Vector3(playerMovement.MovementVector.x, 0.0f, playerMovement.MovementVector.z);
        while (x <= 250 && ( HorizontalMovementVector.magnitude > playerMovement.m_RunningSpeed)) 
        {
            //Lerp'n Slerp towards a target velocity
            decelerationAmount += decelerationStep;
            velocity = Mathf.Lerp(HorizontalMovementVector.magnitude, playerMovement.m_RunningSpeed, decelerationAmount);//playerMovement.MovementVector.magnitude
            HorizontalMovementVector = HorizontalMovementVector.normalized * velocity;
            yield return new WaitForFixedUpdate();
            x++;
            print("decelerating");
        }
        print("decelerated");
        
        
    }
}
