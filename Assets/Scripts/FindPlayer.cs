using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FindPlayer : MonoBehaviour
{
    private void Awake()
    {
        gameObject.GetComponent<CinemachineFreeLook>().m_Follow = GameObject.FindGameObjectWithTag("Player").transform;
        gameObject.GetComponent<CinemachineFreeLook>().m_LookAt = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
