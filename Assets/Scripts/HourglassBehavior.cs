using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourglassBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject Hourglass;
    public float RotationSpeed =  0.1f;
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
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") StartCoroutine(RotateHourglass(180));
    }

    IEnumerator RotateHourglass(int angle)
    {
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
            yield return null;
        }
    }
}
