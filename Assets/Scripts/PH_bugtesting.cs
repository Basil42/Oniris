using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PH_bugtesting : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private float time;

    private Vector3 posOffset;
    private Vector3 tempPos;

    private void Start()
    {
        posOffset = transform.position;
    }

    private void Update()
    {
        time += Time.deltaTime;

        tempPos = posOffset;
        tempPos.x += Mathf.Sin(time * Mathf.PI * frequency) * amplitude;
        

        transform.position = tempPos;
    }
}
