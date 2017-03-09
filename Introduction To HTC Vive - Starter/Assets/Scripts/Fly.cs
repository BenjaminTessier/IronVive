using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour {

    public SteamVR_TrackedObject leftTrackedObj;
    public SteamVR_TrackedObject rightTrackedObj;
    public Transform head;
    public AudioClip takeOff;
    public AudioClip landing;
    public int energy;

    private bool isFlying;
    private bool isGoingDown;
    private bool wasGoingDown;
    private Vector3 initialPosition;
    private Rigidbody body;
    private ConstantForce movement;
    private AudioSource source;
    private float velocity;
    private RigidbodyConstraints originalConstraints;

    private const int ENERGY_MAX = 10000;
    private const int ENERGY_RESTORE = 3;
    private const int ENERGY_HOVER = 2;
    private const int ENERGY_GO_UP = 5;
    private const float factorSpeed = 5F;
    private const float maxSpeed = 20F;

    private SteamVR_Controller.Device leftController
    {
        get { return SteamVR_Controller.Input((int)leftTrackedObj.index); }
    }

    private SteamVR_Controller.Device rightController
    {
        get { return SteamVR_Controller.Input((int)rightTrackedObj.index); }
    }

    void LetsFly()
    {
        isFlying = true;
        source.PlayOneShot(takeOff, 1F);
        source.Play();
    }

    void LetsLand()
    {
        isFlying = false;
        source.Stop();
        source.PlayOneShot(landing, 1F);
    }

    void RestoreEnergy()
    {
        //Debug.Log("restauration de l'énergie");
        if (energy < ENERGY_MAX)
        {
            energy += ENERGY_RESTORE;
        }
    }

    void Awake()
    {
        source = GetComponent<AudioSource>();
        body = GetComponent<Rigidbody>();
        movement = GetComponent<ConstantForce>();
        originalConstraints = body.constraints;
    }

    // Use this for initialization
    void Start () {
        initialPosition = transform.position;
        isFlying = false;
        energy = ENERGY_MAX;
        //source.clip = flying;

        
    }
	
	// Update is called once per frame
	void Update () {
        // A SUPPRIMER
        //energy = ENERGY_MAX;

        isGoingDown = (body.velocity.y < 0);

        body.constraints = originalConstraints;
        velocity = body.velocity.magnitude;

        // on utilise les 2 mains pour monter en altitude
        if (leftController.GetPress(SteamVR_Controller.ButtonMask.Grip) && rightController.GetPress(SteamVR_Controller.ButtonMask.Grip) && ((energy > 0 && isFlying) || energy > 100))
        {
            if (!isFlying)
            {
                LetsFly();
            }

            energy -= ENERGY_GO_UP;

            Vector3 leftDir = head.position - leftTrackedObj.transform.position;
            Vector3 rightDir = head.position - rightTrackedObj.transform.position;
            Vector3 dir = leftDir + rightDir;

            //transform.position += (dir * .025F);
            //body.AddForce(dir * 10F);
            movement.force = dir * factorSpeed;
        }
        // on utilise une main pour maintenir l'altitude / se déplacer latéralement
        else if ((leftController.GetPress(SteamVR_Controller.ButtonMask.Grip) || rightController.GetPress(SteamVR_Controller.ButtonMask.Grip)) && ((energy > 0 && isFlying) || energy > 100))
        {
            if (!isFlying)
            {
                LetsFly();
            }

            energy -= ENERGY_HOVER;

            Vector3 leftDir = new Vector3(0, 0, 0);
            Vector3 rightDir = new Vector3(0, 0, 0);

            if (leftController.GetPress(SteamVR_Controller.ButtonMask.Grip))
            {
                leftDir = head.position - leftTrackedObj.transform.position;
            }

            if (rightController.GetPress(SteamVR_Controller.ButtonMask.Grip))
            {
                rightDir = head.position - rightTrackedObj.transform.position;
            }

            Vector3 dir = leftDir + rightDir;
            Vector3 horizontalDir = new Vector3(dir.x, 0, dir.z);

            //transform.position += (horizontalDir * .04F);
            //body.AddForce(new Vector3(dir.x, 0, dir.z)* 10F);

            // cas de stabilisation sur l'axe y
            if (isGoingDown != wasGoingDown)
            {
                body.constraints = originalConstraints | RigidbodyConstraints.FreezePositionY;
                movement.force = new Vector3(dir.x, 0, dir.z) * factorSpeed;
            }
            // cas où l'on est encore en montée
            else if (!isGoingDown)
            {
                movement.force = new Vector3(dir.x, 0, dir.z) * factorSpeed;
            }
            else
            {
                movement.force = new Vector3(dir.x, 1F, dir.z) * factorSpeed;
            }

        }
        // effet de gravité pour descendre
        else if (transform.position.y > initialPosition.y)
        {
            if (isFlying)
            {
                LetsLand();
            }

            movement.force = new Vector3(0, 0, 0);
            //Vector3 dir = new Vector3(0, -1, 0);
            //transform.position += (dir * .1F);
        }
        // cas où l'on reste à terre
        else
        {
            if (isFlying)
            {
                LetsLand();
            }
            //RestoreEnergy();

            movement.force = new Vector3(0, 0, 0);
        }

        if (velocity == 0F && !leftController.GetPress(SteamVR_Controller.ButtonMask.Grip) && !rightController.GetPress(SteamVR_Controller.ButtonMask.Grip))
        {
            RestoreEnergy();
        }

        // on renseigne la chute/montée par la frame suivante pour déterminer la lévitation,
        wasGoingDown = (body.velocity.y < 0);

        // on limite la vitesse
        if (body.velocity.magnitude > maxSpeed)
        {
            body.velocity = body.velocity.normalized * maxSpeed;
        }
    }
}
