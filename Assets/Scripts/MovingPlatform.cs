﻿using System.Collections;
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
    [HideInInspector]public Vector3 deltaPosition;

    private GameObject player;
   

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        targetPosition = new Vector3(directionX, directionY, directionZ);
        player = GameObject.FindGameObjectWithTag("Player").transform.parent.gameObject;
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if(timer >= loopTime)
        {
            targetPosition *= -1;
            timer = 0;
        }
        deltaPosition = targetPosition * speed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + deltaPosition);
        //rb.velocity = Vector3.zero;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
             other.gameObject.GetComponent<PlayerMovement>().m_externalMoveInfluence +=  deltaPosition;
            
        }
    }

   //public void delete()
   // {
   //     Destroy(gameObject);
   // } 
}
