using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class ClouetSoundManager : MonoBehaviour
{
    private Animator m_animator;
    private AudioSource m_sourceGeneral;
    private AudioSource m_SpeedSource;
    public AudioClip Jump;
    [Range(0.0f,1.0f)]
    public float JumpVolume;
    public AudioClip Blink;
    [Range(0.0f, 1.0f)]
    public float BlinkVolume;
    public AudioClip Landing;
    [Range(0.0f, 1.0f)]
    public float LandingVolume;
    public AudioClip Step;
    [Range(0.0f, 1.0f)]
    public float StepVolume;
    public AudioClip Speed;
    [Range(0.0f, 1.0f)]
    public float SpeedVolume;

    private void Awake()
    {
        m_sourceGeneral = GetComponent<AudioSource>();
        m_animator = GetComponent<Animator>();
        m_sourceGeneral.clip = Speed;
        m_SpeedSource = GetComponents<AudioSource>()[1];
        m_SpeedSource.clip = Speed;
        m_SpeedSource.volume = 0.0f;
    }
   

    
    void FixedUpdate()
    {
        m_SpeedSource.volume = m_animator.GetFloat("speed") * SpeedVolume;
    }

    public void JumpSound()
    {
        if(Jump != null)m_sourceGeneral.PlayOneShot(Jump,JumpVolume);
    }
    public void BlinkSound()
    {
        if(Blink != null)m_sourceGeneral.PlayOneShot(Blink);
    }
    public void LandingSound()
    {
        if (Landing != null) m_sourceGeneral.PlayOneShot(Landing);
    }
    public void StepSound()
    {
        if (Step != null) m_sourceGeneral.PlayOneShot(Step);
    }
    
}
