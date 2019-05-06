using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallTime = 1;
    public float respawnTime = 1;
    private Rigidbody rb;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        rb.freezeRotation = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && rb.useGravity == false)
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
        transform.rotation = originalRotation;
    }
}
