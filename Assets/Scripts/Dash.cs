using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private PlayerMovement PlayerMovement;
    public float speed = 1;
    public float cooldown = 2;
    private float timer;
    private float originalVelocityX;
    private float originalVelocityZ;
    public float decelerationDelay = 1;
    public float decelerationAmount = 0.1f;



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

            originalVelocityX = PlayerMovement.MovementVector.x;
            originalVelocityZ = PlayerMovement.MovementVector.z;

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
        for (int x = 0; x < 10; x++) //Instead of a certain number of loops, go until reach target velocity
        {
            //Lerp'n Slerp towards a target velocity
            //One thing: Might want to mess with original velocity a bit, it can be wonky when you quickly change direction before blinking
            PlayerMovement.MovementVector.x = Mathf.Lerp(PlayerMovement.MovementVector.x, originalVelocityX, decelerationAmount);
            PlayerMovement.MovementVector.z = Mathf.Lerp(PlayerMovement.MovementVector.z, originalVelocityZ, decelerationAmount);
            yield return new WaitForFixedUpdate();
        }
        PlayerMovement.setBusy(false);
    }
}
