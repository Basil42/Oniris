using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    //private Sound menuTheme;
    //private Sound stageTheme;
    

    public static AudioManager instance;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject); //Making sure that the theme will not play more than once at the same time

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        //menuTheme = Array.Find(sounds, sound => sound.name == "MenuTheme");
        //stageTheme = Array.Find(sounds, sound => sound.name == "StageTheme");
    }

    void Update()
    {
        //Code for testing sound
        if(Input.GetKeyDown(KeyCode.T))
        {
            Sound s = sounds[0];
            if (s == null)
            {
                Debug.LogWarning("Sound " + "not found!");
                return;
            }
            s.source.Play();
        }


        //This code, or something like it, should be used when we have more music
        //if (SceneManager.GetActiveScene().buildIndex < 2)
        //{
        //    if (!menuTheme.source.isPlaying)
        //    {
        //        stageTheme.source.Stop();
        //        Play("MenuTheme");
        //    }
        //}
        //else if (!stageTheme.source.isPlaying)
        //{
        //    menuTheme.source.Stop();
        //    Play("StageTheme");
        //}

    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }
        s.source.Play();
    }
}
