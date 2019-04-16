using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cel : MonoBehaviour
{
    private Light light = null;

    private void OnEnable()
    {
        light = this.GetComponent<Light>();
    }

    private void Update()
    {
        shader.SetGlobalVector("_CelDirection", -this.transform.forward);
    }
}