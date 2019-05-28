using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeTimer : MonoBehaviour
{
    public float ChallengeDuration;
    private float ChallengeTimer;
    private bool ChallengeActive;

    public GameObject[] ChallengeWalls;

    void Start()
    {
        SetWalls(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (ChallengeActive)
        {
            ChallengeTimer += Time.deltaTime;
            if (ChallengeTimer >= ChallengeDuration)
            {
                //Turn off stuff
                SetWalls(true);
                ChallengeActive = false;
                ChallengeTimer = 0;
            }
        }
    }

    private void ActivateChallenge()
    {
        //Turn on stuff
        SetWalls(false);
        ChallengeActive = true;
        ChallengeTimer = 0;
    }

    private void SetWalls(bool enabled)
    {
        foreach (GameObject wall in ChallengeWalls)
        {
            wall.SetActive(enabled);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ActivateChallenge();
        }
    }
}
