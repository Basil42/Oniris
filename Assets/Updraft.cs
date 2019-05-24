using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Updraft : MonoBehaviour
{

    [SerializeField] private float m_updraftStrength = 0.1f;

    private Vector3 m_direction;

    [SerializeField] private bool smoothY;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().MovementVector += transform.up * m_updraftStrength;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && smoothY == true)
        {
            //halve it for smoothness?
            other.gameObject.GetComponent<PlayerMovement>().MovementVector.y = other.gameObject.GetComponent<PlayerMovement>().MovementVector.y / 1.5f;
        }
    }
}
