using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Experimental.VFX;

public class SetVFXParameters : MonoBehaviour
{
    private VisualEffect effect;
    private GameObject targetPosition;
    public bool following;
    public float destroyTime;
    //public float lifeTime; might want this? not sure yet
    private float m_attractiveForce;
    public float attractionDelay =0.3f;

    // Start is called before the first frame update
    void Start()
    {
        effect = GetComponent<VisualEffect>();
        targetPosition = GameObject.FindGameObjectWithTag("Player");
        effect.Play();
        if (following)
        {
            m_attractiveForce = effect.GetFloat("PullStrength");
            effect.SetFloat("PullStrength", 0.0f);
            Invoke("startAttraction", attractionDelay);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (following)
        {
            effect.SetVector3("attractive target position", (targetPosition.transform.position + new Vector3(0, 1, 0)));
        }
    }

    public void StopEffect()
    {
        effect.Stop();
        Debug.Log("vfx stop");
        Invoke("destroyObj", destroyTime);
    }

    private void startAttraction()
    {
        effect.SetFloat("PullStrength", m_attractiveForce);
    }

    private void destroyObj()
    {
        Destroy(gameObject);
    }
}
