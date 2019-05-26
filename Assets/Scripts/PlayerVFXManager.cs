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
    private Vector3 offset;
    [SerializeField] float SpeedTreshold = 0.05f;
    [SerializeField] int spawnRate = 10;
    //VisualEffect RunEffect2;
    // Start is called before the first frame update
    void Start()
    {
        RunEffect = RunEffectObject.GetComponent<VisualEffect>();
        offset = RunEffect.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        RunUpdate();
        oldPosition = RunEffectObject.transform.position;
        if(deltaPosition.magnitude < SpeedTreshold)
        {
            RunEffect.SetInt("spawnrate",0);
        }
        else
        {
            RunEffect.SetInt("spawnrate", spawnRate);
        }
    }

    private void RunUpdate()
    {
        RunEffectObject.transform.position = transform.position + offset;
        deltaPosition = oldPosition - RunEffectObject.transform.position;
        RunEffect.SetVector3("DeltaPos",deltaPosition );
    }
}
