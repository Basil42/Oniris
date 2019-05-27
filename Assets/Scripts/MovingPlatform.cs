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
    private Vector3 originalPostion;

    public bool automatic = true;
    private bool active = false;

    public float loopTime;
    private float timer;
    [HideInInspector]public Vector3 deltaPosition;

    private GameObject player;
   

    // Start is called before the first frame update
    void Start()
    {
        originalPostion = transform.position;
        rb = gameObject.GetComponent<Rigidbody>();
        targetPosition = new Vector3(directionX, directionY, directionZ);
        player = GameObject.FindGameObjectWithTag("Player").transform.parent.gameObject;
    }

    private void FixedUpdate()
    {
        if(automatic || active)
        {
            timer += Time.fixedDeltaTime;
            if (timer >= loopTime)
            {
                targetPosition *= -1;
                timer = 0;
            }
            deltaPosition = targetPosition * speed * Time.fixedDeltaTime;
            rb.MovePosition(transform.position + deltaPosition);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
             other.gameObject.GetComponent<PlayerMovement>().m_externalMoveInfluence +=  deltaPosition;
            
        }
    }

    public void activate()
    {
        if(active == false)
        {
            print("Hello there");
            active = true;
            Invoke("deactivate", loopTime);
        }
    }

    private void deActivate()
    {
        active = false;
        transform.position = originalPostion;
    }
}
