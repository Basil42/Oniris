using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Experimental.VFX;

public class SetVFXParameters : MonoBehaviour
{
    private VisualEffect effect;
    private GameObject targetPosition;
    public bool following;

    // Start is called before the first frame update
    void Start()
    {
        effect = GetComponent<VisualEffect>();
        targetPosition = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (following)
        {
            effect.SetVector3("attractive target position", (targetPosition.transform.position + new Vector3(0, 1, 0)));
            effect.Play();
        }
        else
        {
            effect.Stop();
        }
    }
}
