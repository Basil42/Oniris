using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
public class PlayerVFXManager : MonoBehaviour
{
    public GameObject RunEffectObject;
    private VisualEffect RunEffect;
    private Vector3 oldPosition;
    private Vector3 deltaPosition;
    //VisualEffect RunEffect2;
    // Start is called before the first frame update
    void Start()
    {
        RunEffect = RunEffectObject.GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        RunUpdate();
        oldPosition = RunEffectObject.transform.position;
    }

    private void RunUpdate()
    {
        RunEffectObject.transform.position = transform.position;
        RunEffect.SetVector3("DeltaPos", oldPosition - RunEffectObject.transform.position);
    }
}
