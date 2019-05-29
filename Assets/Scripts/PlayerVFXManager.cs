using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
public class PlayerVFXManager : MonoBehaviour
{
    public GameObject RunEffectObject;
    public GameObject BlinkEffectObject;
    public GameObject AmbiantDustObject;
    private VisualEffect RunEffect;
    private VisualEffect BlinkEffect;
    private VisualEffect AmbiantEffect;
    private Vector3 oldPosition;
    private Vector3 deltaPosition;
    private Vector3 SpeedEffectoffset;
    [SerializeField] private Vector3 BlinkEffectTargetOffset = Vector3.zero;
    [SerializeField] float SpeedTreshold = 0.05f;
    [SerializeField] int spawnRate = 10;
    //VisualEffect RunEffect2;
    // Start is called before the first frame update
    void Start()
    {
        RunEffect = RunEffectObject.GetComponent<VisualEffect>();
        BlinkEffect = BlinkEffectObject.GetComponent<VisualEffect>();
        SpeedEffectoffset = RunEffect.transform.localPosition;
        AmbiantEffect = AmbiantDustObject.GetComponent<VisualEffect>();
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
        BlinkEffectObject.transform.position = transform.position;
        BlinkEffect.SetVector3("attractive target position", transform.position + BlinkEffectTargetOffset);
        AmbiantDustObject.transform.position = transform.position;
    }

    private void RunUpdate()
    {
        RunEffectObject.transform.position = transform.position + SpeedEffectoffset;
        deltaPosition = oldPosition - RunEffectObject.transform.position;
        RunEffect.SetVector3("DeltaPos",deltaPosition );
    }
    public void PlayBlink()
    {
        BlinkEffect.SendEvent("Spawn");
    }
}
