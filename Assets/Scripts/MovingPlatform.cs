using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    private Rigidbody rb;

    public float speed;
    public int directionX;
    public int directionY;
    public int directionZ;
    private Vector3 targetPosition;

    public float loopTime;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        targetPosition = new Vector3(directionX, directionY, directionZ);
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if(timer >= loopTime)
        {
            targetPosition *= -1;
            timer = 0;
        }
        rb.MovePosition(transform.position + targetPosition * speed * Time.fixedDeltaTime);
    }
}
