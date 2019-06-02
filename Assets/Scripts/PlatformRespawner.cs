using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRespawner : MonoBehaviour
{
    public GameObject m_platform;
    public int m_platformDirectionX;
    public int m_platformDirectionY;
    public int m_platformDirectionZ;
    public float m_platformSpeed;
    public float m_respawnInterval;
    public float m_despawnTime;

    private float m_timer;
    private GameObject m_currentplatform;
    private MovingPlatform m_currentplatformScript;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(m_timer > m_respawnInterval)
        {
            print("Spawnplatform");
            spawnPlatform();
            m_timer = 0;
        }
        m_timer += Time.fixedDeltaTime;
    }

    private void spawnPlatform()
    {
        m_currentplatform = Instantiate(m_platform, transform.position, transform.rotation);
        m_currentplatformScript = m_currentplatform.GetComponent<MovingPlatform>();
        m_currentplatformScript.directionX = m_platformDirectionX;
        m_currentplatformScript.directionY = m_platformDirectionY;
        m_currentplatformScript.directionZ = m_platformDirectionZ;
        m_currentplatformScript.speed = m_platformSpeed;
        m_currentplatformScript.loopTime = m_despawnTime + 1; //hack to make it never loop
        StartCoroutine(removePlatform(m_despawnTime, m_currentplatform));
    }

    private IEnumerator removePlatform(float despawnTime, GameObject obj)
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(obj);
    }
}
