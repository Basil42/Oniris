using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallTime = 1;
    public float respawnTime = 1;
    private Rigidbody rb;
    private Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        originalPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Invoke("fall", fallTime);
        }
    }

    private void fall()
    {
        rb.useGravity = true;
        rb.constraints = ~RigidbodyConstraints.FreezeAll;
        Invoke("reset", respawnTime);
    }

    private void reset()
    {
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        transform.position = originalPosition;
    }
}
