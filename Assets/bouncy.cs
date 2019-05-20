using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncy : MonoBehaviour
{
    private PlayerMovement movement;
    [SerializeField] private float bounciness = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        movement = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO: If we need to, we can set the double jump flag if it turns out to be unreliable.
        if (other.gameObject.tag == "Player")
        {
            movement.MovementVector.y = bounciness;
        }
    }
}
