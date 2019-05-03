using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class check : MonoBehaviour
{
    public checkContent m_content;
    //vfx and tutorial images(tied to content)
    private CheckManager m_manager;
    // Start is called before the first frame update
    void Awake()
    {
        m_manager = GameObject.FindGameObjectWithTag("checkManager").GetComponent<CheckManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
