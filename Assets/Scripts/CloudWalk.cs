using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudWalk : MonoBehaviour
{
    public GameObject Cloud;
    private GameObject CloudInstance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CloudInstance = Instantiate(Cloud, other.gameObject.transform);
        }
    }

   
}
