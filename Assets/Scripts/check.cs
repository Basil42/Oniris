using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.VFX;
public class check : MonoBehaviour
{
    public checkContent m_content;
    public string textPrompt = "This is a test. Or the devs forgot to remove it.";
    [Tooltip("Should never ever be duplicated")]
    public int index =-1;
    public AudioClip pickUpSound;
    [Range(0.0f, 1.0f)]
    public float m_pickUpVolume;
    //vfx and tutorial images(tied to content)
    private CheckManager m_manager;
    private VisualEffect m_passive;
    private VisualEffect m_pickup;
    private musicManager m_musicManager;
    private CheckCounter m_counter;
    [SerializeField] private float dialogueTime = 10;
    // Start is called before the first frame update
    void Awake()
    {
        //if (index == -1) Debug.LogError("cannot have check without proper index");
        //m_manager = GameObject.FindGameObjectWithTag("checkManager").GetComponent<CheckManager>();
        //m_content = m_manager.getContent(index, out textPrompt);
        m_musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<musicManager>();
        m_counter = GameObject.FindGameObjectWithTag("CheckCounter").GetComponent<CheckCounter>();
    }
    private void Start()
    {
        m_passive = GetComponentsInChildren<VisualEffect>()[0];
        m_pickup = GetComponentsInChildren<VisualEffect>()[1];
        m_passive.SetFloat("ExplosionForce", 0.0f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            CallDialogue(textPrompt);
            switch (m_content)
            {
                case checkContent.Dash_fragment:
                    
                    break;
                case checkContent.Double_jump:
                    player.m_abilityFlags |= AbilityAvailability.hasDoublejump;
                    
                    GameObject.FindGameObjectWithTag("DoubleJumpUI").GetComponent<Image>().fillAmount = 1;
                    m_musicManager.StartCoroutine(m_musicManager.ChoirFadeIn());
                    break;
                case checkContent.Blink:
                    player.m_abilityFlags |= AbilityAvailability.hasBlink;
                    
                    GameObject.FindGameObjectWithTag("BlinkUI").GetComponent<Image>().fillAmount = 1;
                    m_musicManager.StartCoroutine(m_musicManager.SynthFadeIn());
                    break;
                case checkContent.Wall_Jump:
                    player.m_abilityFlags |= AbilityAvailability.hasWallJump;
                    
                    GameObject.FindGameObjectWithTag("WallJumpUI").GetComponent<Image>().fillAmount = 1;
                    m_musicManager.StartCoroutine(m_musicManager.FluteFadeIn());
                    break;
                default:
                    Debug.LogError("Check has no content, ensure that its index is valid.");
                    break;
            }
        }
        GetComponent<Collider>().enabled = false;
        m_passive.Stop();
        m_passive.SetFloat("ExplosionForce", 10.0f);
        m_pickup.SendEvent("PickUp");
        Invoke("stopPickup", 1.0f);
        m_counter.addCheck();
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().PlayOneShot(pickUpSound, m_pickUpVolume);
    }

    private void stopPickup()
    {
        m_pickup.Stop();
    }

    private void CallDialogue(string text)
    {
        //Might want to optimise finding the dialogue system
        GameObject.FindGameObjectWithTag("DialogueSystem").GetComponent<DialougeSystem>().StartDialogue(text, dialogueTime);
    }
}
