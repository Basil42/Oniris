using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class CloudWalk : MonoBehaviour
{
    [SerializeField] private GameObject Cloud;
    private GameObject CloudInstance;
    private float m_Timer;
    public float m_Duration = 5.0f;
    private float m_targetIntensity = 0.0f;
    private float m_StartingIntensity = 3.0f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CloudInstance = Instantiate(Cloud, other.gameObject.transform);
            CloudInstance.GetComponent<VisualEffect>().SetFloat("Intensity", m_StartingIntensity);
            m_Timer = 0.0f;
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<VisualEffect>().Stop();
            StartCoroutine(PlatformBehavior());
        }
    }

    private void FixedUpdate()
    {
        if(CloudInstance != null && transform.parent.tag == "Player")
        {
            switch (CloudInstance.transform.parent.GetComponent<PlayerMovement>().m_state)
            {
                case movementState.jumping:
                case movementState.doubleJumping:
                case movementState.dashing:
                case movementState.blinking:
                    CloudInstance.transform.parent = null;
                    break;
                
                default:
                    break;
            }
        }
    }

    private IEnumerator PlatformBehavior()
    {
        while(m_Timer < m_Duration)
        {
            m_Timer += Time.fixedDeltaTime;
            CloudInstance.GetComponent<VisualEffect>().SetFloat("Intensity", Mathf.SmoothStep(m_StartingIntensity, m_targetIntensity, m_Timer / m_Duration));
            yield return new WaitForFixedUpdate();
        }
        Destroy(CloudInstance);
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<VisualEffect>().Reinit();
    }
   
}
