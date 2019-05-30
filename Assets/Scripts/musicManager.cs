using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class musicManager : MonoBehaviour
{
    public AudioMixer mixer;
    private PlayerMovement m_player;
    private float m_MasterFadeSpeed = 0.2f;
    private float m_MasterVolume;
    public float m_targetMasterVolume;
    private float m_SynthVolume;
    private float m_targetSynthVolume;
    private float m_FluteVolume;
    private float m_targetFluteVolume;
    private float m_ChoirVolume;
    private float m_targetChoirVolume;
    private float m_SynthFadeSpeed = 0.05f;
    private float m_ChoirFadeSpeed = 0.05f;
    private float m_FluteFadeSpeed = 0.05f;

    void Start()
    {
        mixer.GetFloat("MasterVol", out m_targetMasterVolume);
        mixer.GetFloat("SynthVol", out m_targetSynthVolume);
        mixer.GetFloat("FluteVol", out m_targetFluteVolume);
        mixer.GetFloat("ChoirVol", out m_targetChoirVolume);
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        mixer.SetFloat("MasterVol", -80.0f);
        mixer.SetFloat("SynthVol", -80.0f);
        mixer.SetFloat("FluteVol", -80.0f);
        mixer.SetFloat("ChoirVol", -80.0f);
        mixer.GetFloat("MasterVol", out m_MasterVolume);
        mixer.GetFloat("SynthVol", out m_SynthVolume);
        mixer.GetFloat("FluteVol", out m_FluteVolume);
        mixer.GetFloat("ChoirVol", out m_ChoirVolume);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mixer.GetFloat("MasterVol", out m_MasterVolume);
        mixer.SetFloat("MasterVol", Mathf.MoveTowards(m_MasterVolume, (Mathf.Clamp01(m_player.MovementVector.magnitude / m_player.m_RunningSpeed) * 40.0f) - 40.0f, m_MasterFadeSpeed));

    }

    public IEnumerator SynthFadeIn()
    {
        float lerpStep = 0.0f;
        while(!Mathf.Approximately(m_SynthVolume, m_targetSynthVolume))
        {
            
            mixer.SetFloat("SynthVol", Mathf.Lerp(-80.0f, m_targetSynthVolume, lerpStep));
            lerpStep += m_SynthFadeSpeed;
            yield return new WaitForFixedUpdate();
            mixer.GetFloat("SynthVol", out m_SynthVolume);
        }
    }

    public IEnumerator FluteFadeIn()
    {
        float lerpStep = 0.0f;
        while (!Mathf.Approximately(m_FluteVolume, m_targetFluteVolume))
        {
            mixer.SetFloat("FluteVol", Mathf.Lerp(-80.0f, m_targetFluteVolume, lerpStep));
            lerpStep += m_FluteFadeSpeed;
            yield return new WaitForFixedUpdate();
            mixer.GetFloat("FluteVol", out m_FluteVolume);
        }
    }

    public IEnumerator ChoirFadeIn()
    {
        float lerpStep = 0.0f;
        while (!Mathf.Approximately(m_ChoirVolume, m_targetChoirVolume))
        {
            mixer.SetFloat("ChoirVol", Mathf.Lerp(-80.0f, m_targetChoirVolume, lerpStep));
            lerpStep += m_ChoirFadeSpeed;
            yield return new WaitForFixedUpdate();
            mixer.GetFloat("ChoirVol", out m_ChoirVolume);
        }
    }
}
