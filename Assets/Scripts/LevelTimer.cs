﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    private static float timer;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    private void Update()
    {
        timer += Time.deltaTime;
        //if(Input.GetKeyDown(KeyCode.A))
        //{
        //    //timer = timer ;
        //    WriteString();
        //}
    }

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    if(scene == Ending)
    //    {
    //      timer = timer / 60;
    //      WriteString();
    //    }
    //}

    public void goal()
    {
        timer = timer / 60;
        WriteString();
    }

    //adapted from https://support.unity3d.com/hc/en-us/articles/115000341143-How-do-I-read-and-write-data-from-a-text-file-
    static void WriteString()
    {
        string path = "Scores/Scores.txt"; //"Assets/Resources/Scores/Scores.txt";

        string score = timer.ToString();

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(score);
        writer.Close();

        //Re-import the file to update the reference in the editor
        //AssetDatabase.ImportAsset(path);
    }

    //static void ReadString()
    //{
    //    string path = "Assets/Resources/test.txt";

    //    //Read the text from directly from the test.txt file
    //    StreamReader reader = new StreamReader(path);
    //    Debug.Log(reader.ReadToEnd());
    //    reader.Close();
    //}
}
