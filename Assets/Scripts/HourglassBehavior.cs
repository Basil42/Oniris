using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourglassBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject Hourglass;
    public float RotationSpeed =  0.1f;
    public float timer = 5.0f;
    private float internalTime = 0.0f;
    private void Awake()
    {
        Hourglass = transform.GetChild(1).gameObject;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        internalTime += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") StartCoroutine(RotateHourglass(180));
    }

    IEnumerator RotateHourglass(int angle)
    {
        internalTime = 0.0f;
        float step = 0.0f;
        float smoothstep = 0.0f;
        float StartingAngle = Hourglass.transform.localRotation.eulerAngles.z;
        Quaternion StartingRotation = Hourglass.transform.rotation;
        Quaternion TargetRotation = StartingRotation * Quaternion.Euler(0.0f, angle, 0.0f);
        while (step < 1.0f)
        {
            
            Hourglass.transform.localRotation = Quaternion.Lerp(StartingRotation, TargetRotation, smoothstep);
            step += RotationSpeed * Time.deltaTime;
            smoothstep = Mathf.SmoothStep(0.0f, 1.0f, step);
            Hourglass.GetComponent<Renderer>().material.SetFloat("_GoldGradient", Mathf.Clamp(smoothstep,0.0f,1.0f));
            yield return null;
        }
        while(internalTime < timer)
        {
            Hourglass.GetComponent<Renderer>().material.SetFloat("_GoldGradient", Mathf.Clamp(-Mathf.Clamp01(internalTime/timer)+1.0f, 0.0f, 1.0f));
            
            yield return null;
        }
        
    }
}
