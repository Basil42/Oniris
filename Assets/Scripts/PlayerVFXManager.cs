using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
public class PlayerVFXManager : MonoBehaviour
{
    public GameObject RunEffectObject;
    public GameObject BlinkEffectObject;
    public Renderer HairRenderer;
    private VisualEffect RunEffect;
    private VisualEffect BlinkEffect;
    private PlayerMovement playerMovement;
    private Vector3 oldPosition;
    private Vector3 deltaPosition;
    private Vector3 SpeedEffectoffset;
    [SerializeField] private Vector3 BlinkEffectTargetOffset;
    [SerializeField] float SpeedTreshold = 0.05f;
    [SerializeField] int spawnRate = 10;
    //VisualEffect RunEffect2;
    private float m_windPrevious = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        RunEffect = RunEffectObject.GetComponent<VisualEffect>();
        BlinkEffect = BlinkEffectObject.GetComponent<VisualEffect>();
        SpeedEffectoffset = RunEffect.transform.localPosition;
        playerMovement = GetComponent<PlayerMovement>();
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
        HairRenderer.material.SetFloat("Vector1_7F45795D", HairRenderer.material.GetFloat("Vector1_7F45795D") + Time.deltaTime * (1.0f +m_windPrevious * 5.0f));
    }
    private void FixedUpdate()
    {
        m_windPrevious = HairRenderer.material.GetFloat("Vector1_F38142DE");
        
        HairRenderer.material.SetFloat("Vector1_F38142DE", Mathf.Lerp(m_windPrevious, Mathf.Clamp01(Vector3.Project(playerMovement.MovementVector, transform.forward).magnitude / playerMovement.m_RunningSpeed),0.05f ));
        
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
