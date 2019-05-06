using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class check : MonoBehaviour
{
    public checkContent m_content;
    [Tooltip("Should never ever be duplicated")]
    public int index;
    //vfx and tutorial images(tied to content)
    private CheckManager m_manager;
    // Start is called before the first frame update
    void Awake()
    {
        m_manager = GameObject.FindGameObjectWithTag("checkManager").GetComponent<CheckManager>();
    }
    private void Start()
    {
        m_content = m_manager.getContent(index);
    }
    

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
        switch (m_content)
        {
            case checkContent.Dash_fragment:
                break;
            case checkContent.Double_jump:
                player.m_abilityFlags |= AbilityAvailability.hasDoublejump;
                break;
            case checkContent.Blink:
                player.m_abilityFlags |= AbilityAvailability.hasBlink;
                break;
            case checkContent.Wall_Jump:
                player.m_abilityFlags |= AbilityAvailability.hasWallJump;
                break;
            default:
                Debug.LogError("Check has no content, ensure that its index is valid.");
                break;
        }
    }
}
