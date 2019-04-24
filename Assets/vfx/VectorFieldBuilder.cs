using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class VectorFieldBuilder : EditorWindow
{
    private const float m_intensity = 1.0f;
    private float m_updraft;
    private int m_resolution;

    static VectorFieldBuilder window;

    private string m_name;
    private float m_pull;

    [MenuItem("Assets/Create/Vector field")]
    static void CreateAsset()
    {
        window = CreateInstance<VectorFieldBuilder>();
        window.ShowUtility();

    }

    private void CreateField()
    {
        Texture3D field = new Texture3D(m_resolution, m_resolution, m_resolution, TextureFormat.RGBA32, true);
        Color[] array = new Color[m_resolution * m_resolution * m_resolution];
        //Color setting for a vortex field, could be extended

        for (int x = 0; x < m_resolution; x++)
        {
            for (int y = 0; y < m_resolution; y++)
            {
                for (int z = 0; z < m_resolution; z++)
                {
                    Vector3 d = new Vector3(x - (m_resolution / 2), 0.0f, z - (m_resolution / 2));
                    Vector3 v = (Vector3.Cross(Vector3.up, d) / d.magnitude) * m_intensity;
                    v = v - d * m_pull;

                    Color c = new Color(v.x, m_updraft, v.z, 1.0f);
                    array[x + (y * m_resolution) + (z * m_resolution * m_resolution)] = c;
                }
            }
        }
        field.SetPixels(array);
        field.Apply();
        field.wrapMode = TextureWrapMode.Clamp;
        string TargetPath = "Assets";

        //example for selecting active project window folder found at https://gist.github.com/allanolivei/9260107
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            TargetPath = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(TargetPath) && File.Exists(TargetPath))
            {
                TargetPath = Path.GetDirectoryName(TargetPath);
                break;
            }
        }

        AssetDatabase.CreateAsset(field, TargetPath + "/" + m_name + ".asset");
    }

    private void OnGUI()
    {
        
        m_updraft = EditorGUILayout.FloatField("Updraft", m_updraft);
        m_pull = EditorGUILayout.FloatField("Inward pull", m_pull);
        m_resolution = EditorGUILayout.IntField("Resolution/size", m_resolution);
        m_name = EditorGUILayout.TextField("name", m_name);
        EditorGUILayout.HelpBox("If no folder is selected, the field Vector asset will be created in the Assets folder", MessageType.Info);


        if (GUILayout.Button("Create"))
        {
            CreateField();
            window.Close();
        }
    }
}
