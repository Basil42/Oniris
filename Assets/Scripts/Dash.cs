using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private PlayerMovement PlayerMovement;
    public float speed = 1;
    public float cooldown = 2;
    private float timer;
    private float targetVelocityX;
    private float targetVelocityZ;
    public float decelerationDelay = 1;
    public float decelerationStep = 0.2f;
    private float decelerationAmount;



    private void Awake()
    {
        PlayerMovement = GetComponent<PlayerMovement>();
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

            PlayerMovement.MovementVector += speed * transform.forward;
            PlayerMovement.MovementVector.y = 0;
            timer = 0;

            transform.rotation = Quaternion.LookRotation(new Vector3(PlayerMovement.MovementVector.x, 0.0f, PlayerMovement.MovementVector.z));

        //coroutine function to reduce speed after having dashed
        StartCoroutine("decelerate");
        }
    }

    private IEnumerator decelerate()
    {
        PlayerMovement.setBusy(true);
        yield return new WaitForSeconds(decelerationDelay);
        int x = 0;

        decelerationAmount = 0;

        while (x <= 250 && (PlayerMovement.MovementVector.magnitude > PlayerMovement.m_RunningSpeed)) 
        {
            //Lerp'n Slerp towards a target velocity
            decelerationAmount += decelerationStep;
            PlayerMovement.MovementVector.x = Mathf.Lerp(PlayerMovement.MovementVector.x, targetVelocityX, decelerationAmount);
            PlayerMovement.MovementVector.z = Mathf.Lerp(PlayerMovement.MovementVector.z, targetVelocityZ, decelerationAmount);
            yield return new WaitForFixedUpdate();
            x++;
            print("decelerating");
        }
        print("decelerated");
        PlayerMovement.setBusy(false);
    }
}
