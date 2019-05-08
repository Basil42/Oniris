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

    // Start is called before the first frame update
    void Start()
    {
        effect = GetComponent<VisualEffect>();
        targetPosition = GameObject.FindGameObjectWithTag("Player");
        effect.Play();
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
        Invoke("destroyObj", destroyTime);
    }

    private void destroyObj()
    {
        Destroy(gameObject);
    }
}
