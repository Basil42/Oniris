using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerMovement))]
public class Blink : MonoBehaviour
{
    PlayerMovement m_playerMovement;

    private CameraController cameraController;
    private CharacterController charCtrl;
    public GameObject blinkStream;
    private GameObject currentBlinkStream;
    public GameObject blinkParticles;
    private GameObject currentBlinkParticles;

    private LayerMask blinkThrough;
    public float distance = 10;
    public float blinkStep = 50;

    private List<Material> dissolveBodies;

    public float blinkDuration = 0.3f;
    private float timer = 0.0f;

    public float dissolveSpeed = 4;
    public float appearSpeed = 1;

    public bool m_BlinkEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        blinkThrough = ~LayerMask.GetMask("Blinkable");// everything but blinkable

        dissolveBodies = new List<Material>();
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            if(renderer.material != null)
            {
                dissolveBodies.Add(renderer.material);
            }
        }
        m_playerMovement = GetComponent<PlayerMovement>();
        charCtrl = GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame

    public void blink(Vector3 inputVector)
    {
        m_playerMovement.controller.enabled = false;
        m_playerMovement.m_abilityFlags &= ~AbilityAvailability.blink;

        StartCoroutine("Dissolve");

        Vector3 blinkDirection = inputVector;
        
        inputVector.y = 0;
        inputVector = inputVector.normalized;//shouldn't be here

        //Velocity, magnitude of movement gets applied in input direction after blink
        RaycastHit hit;
        RaycastHit hit2;

        if (m_playerMovement.m_state == movementState.grounded)
        {
            print("grounded Blink");
            Physics.Raycast(transform.position + charCtrl.center, Vector3.down, out hit2, distance);
            blinkDirection = Vector3.ProjectOnPlane(inputVector, hit2.normal).normalized;
        }

        //spherecast starts from center of player, slightly behind them in the opposite direction of the inputVector
        Vector3 p1 = transform.position + charCtrl.center - blinkDirection;
        Physics.SphereCast(p1, charCtrl.height / 2, blinkDirection, out hit, distance, blinkThrough);

        m_playerMovement.m_state = movementState.blinking;
        SpawnBlinkVFX();

        if (hit.distance == 0)//dangerous ?
        {
            //Raycast to check for blinkable colliders. 
            RaycastHit hit3;
            Physics.Raycast(transform.position + new Vector3(0, charCtrl.height / 2, 0), blinkDirection, out hit3, distance);

            //Checking whether there is a blinkable collider, and if the target blink position is inside it.
            if (hit3.collider == true && hit3.collider.gameObject.layer == 8 && hit3.collider.bounds.Contains(transform.position + inputVector * distance))
            {
                //If the target position is inside the collider, use the raycast that checks for blinkables as distance instead
                StartCoroutine(Blinking(inputVector, transform.position + blinkDirection * (hit3.distance - 0.5f)));
            }
            //No blinkable collider was found, or the target position was beyond it. Continue as normal, blink full distance
            else StartCoroutine(Blinking(inputVector, transform.position + blinkDirection * distance));
        }
        else
        {
            //Found a non-blinkable collider, position uses the hit.distance minus a small offset
            StartCoroutine(Blinking(inputVector, transform.position + blinkDirection * (hit.distance - 0.5f)));
            print(hit.distance);
        }
    }

    private IEnumerator Blinking(Vector3 inputVector, Vector3 targetPosition)
    {
        //Deciding momentum upon leaving the blink
        float mag = m_playerMovement.MovementVector.magnitude;
        m_playerMovement.MovementVector.x = inputVector.x;
        m_playerMovement.MovementVector.z = inputVector.z;
        m_playerMovement.MovementVector.y = 0;
        m_playerMovement.MovementVector = inputVector.normalized * mag;
        
        //Blinking
        timer = 0.0f;
        while (timer <= blinkDuration)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, blinkStep * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
        }

        StartCoroutine("Appear");
        EndBlinkVFX();

        m_playerMovement.controller.enabled = true;
        m_playerMovement.GroundCheck();
        print("Finished blinking");
    }

    private IEnumerator Dissolve()
    {
        float dissolveAmount = 0;
        while(dissolveAmount < 1)
        {
            foreach(Material mat in dissolveBodies)
            {
                mat.SetFloat("_Dissolve_Value", dissolveAmount);   
            }
            dissolveAmount += Time.fixedDeltaTime * dissolveSpeed;
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator Appear()
    {
        float dissolveAmount = 1;
        while (dissolveAmount > 0)
        {
            foreach (Material mat in dissolveBodies)
            {
                mat.SetFloat("_Dissolve_Value", dissolveAmount);
            }
            dissolveAmount -= Time.fixedDeltaTime * appearSpeed;
            yield return new WaitForFixedUpdate();
        }
    }

    private void SpawnBlinkVFX()
    {
        currentBlinkStream = Instantiate(blinkStream, transform.position, transform.rotation);
        currentBlinkParticles = Instantiate(blinkParticles, transform.position, transform.rotation);
    }

    private void EndBlinkVFX()
    {
        currentBlinkStream.GetComponent<SetVFXParameters>().StopEffect();
        currentBlinkParticles.GetComponent<SetVFXParameters>().StopEffect();
    }
}
