using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Weapons : MonoBehaviour
{
    public GameObject projectile;
    public Transform head;
    public AudioClip laserSound;
    public AudioClip beamUp;
    public AudioClip beamFiring;
    public AudioClip beamDown;
    public AudioClip repulseUp;
    public AudioClip repulseFiring;
    public AudioClip repulseDown;
    public bool rightHand;
    
    private int currentWeaponMode;
    private int previousWeaponMode;
    private bool superWeaponIsEnabled;
    private bool superWeaponWasEnabled;

    private float test;
    private SteamVR_TrackedObject trackedObj;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitPoint;
    private Transform laserCube;
    private bool freeFire;
    private int weaponState;
    private LineRenderer laserLine;
    private Vector3 controllerPosition;
    private Quaternion controllerRotation;
    private AudioSource source;
    private bool smallFireTest = false;

    private const int MODE1 = 0;
    private const int MODE2 = 1;

    private const int WAITING = 0;
    private const int LOADING = 1;
    private const int LOADED = 2;
    private const int FIRING = 3;
    private const int UNLOADING = 4;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private void UpYourBeam()
    {
        //Debug.Log("on commence à tirer");
        source.PlayOneShot(beamUp, 1F);
        StartCoroutine(WaitUntilBeamIsUp());
    }

    private void DownYourBeam()
    {
        //Debug.Log("on arrête de tirer");
        source.Stop();
        laserLine.enabled = false;
        source.PlayOneShot(beamDown, 1F);
        weaponState = WAITING;
    }

    IEnumerator WaitUntilBeamIsUp()
    {
        //Debug.Log("on commence à charger");
        weaponState = LOADING;

        yield return new WaitForSeconds(beamUp.length / 1.85F);

        // on ne passe à l'état chargé que si le chargement n'a pas été interrompu entre temps
        if (weaponState == LOADING)
        {
            weaponState = LOADED;
        }

        //Debug.Log("chargement terminé");
    }

    private void UpYourRepulse()
    {
        //Debug.Log("on commence à tirer");
        source.PlayOneShot(repulseUp, 8F);
        StartCoroutine(WaitUntilRepulseIsUp());
    }
    
    private void DownYourRepulse()
    {
        //Debug.Log("on arrête de tirer");
        source.Stop();
        laserLine.enabled = false;
        source.PlayOneShot(repulseDown, 8F);
        StartCoroutine(WaitUntilRepulseIsDown());
    }

    IEnumerator TestSmallFireTime()
    {
        smallFireTest = true;
        yield return new WaitForSeconds(repulseFiring.length / 5F);
        laserLine.enabled = false;
        smallFireTest = false;
    }

    IEnumerator WaitUntilRepulseIsUp()
    {
        //Debug.Log("on commence à charger");
        weaponState = LOADING;

        yield return new WaitForSeconds(repulseUp.length);

        // on ne passe à l'état chargé que si le chargement n'a pas été interrompu entre temps
        if (weaponState == LOADING)
        {
            weaponState = LOADED;
        }

        //Debug.Log("chargement terminé");
    }

    IEnumerator WaitUntilRepulseHasFired()
    {
        weaponState = FIRING;
        StartCoroutine(TestSmallFireTime());
        yield return new WaitForSeconds(repulseFiring.length);
        DownYourRepulse();
    }
    
    IEnumerator WaitUntilRepulseIsDown()
    {
        weaponState = UNLOADING;
        yield return new WaitForSeconds(repulseDown.length / 2F);
        weaponState = WAITING;
    }

    IEnumerator WaitForFireRate()
    {
        freeFire = false;
        yield return new WaitForSeconds(laserSound.length / 5F);
        freeFire = true;
    }

    private void HoldYourFire()
    {
        if (superWeaponWasEnabled)
        {
            DownYourBeam();
        }
        else
        {
            DownYourRepulse();
        }
    }

    private void AbortFiring()
    {
        laserLine.enabled = false;
        weaponState = WAITING;
        //Debug.Log("halte au feu");
    }

    private void ShowLaser()
    {
        RaycastHit hit;
        Vector3 distanceToArm = new Vector3(1.5F, 0, 0);
        Vector3 positionToArm = transform.rotation * distanceToArm;
        Transform originRepulseGun = trackedObj.transform.FindChild("repulseCube");

        int layerMask = 1 << 2;
        layerMask = ~layerMask;

        laserLine.SetPosition(0, originRepulseGun.position);

        if (Physics.Raycast(originRepulseGun.position, originRepulseGun.TransformDirection(new Vector3(0, -1, 0.05F)), out hit, 1000, layerMask))
        {
            laserLine.SetPosition(1, hit.point);
            Health health = hit.collider.GetComponent<Health>();

            if (health != null)
            {
                //Debug.Log("touché");
                health.Damage(1);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * 100F);
            }
        }
        else
        {
            laserLine.SetPosition(1, originRepulseGun.position + originRepulseGun.TransformDirection(new Vector3(0, -1, 0.05F)) * 1000F);
        }
        laserLine.enabled = true;
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        laserCube = trackedObj.transform.FindChild("laserCube");
        source = GetComponent<AudioSource>();
        source.clip = beamFiring;
    }

    // Use this for initialization
    void Start()
    {

        currentWeaponMode = MODE1;
        freeFire = true;
        superWeaponIsEnabled = false;
        superWeaponWasEnabled = false;
        weaponState = WAITING;

        laserLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        #region gestion modes de tir

        // sauvegarde des états du frame précédent
        previousWeaponMode = currentWeaponMode;
        superWeaponWasEnabled = superWeaponIsEnabled;

        // on change de mode de tir uniquement si on n'est pas en train de tirer
        if (!Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            controllerRotation = Quaternion.FromToRotation(transform.InverseTransformVector(head.up), transform.InverseTransformVector(trackedObj.transform.up));

            controllerPosition = controllerRotation.eulerAngles;

            test = controllerPosition.z;

            if ((rightHand && controllerPosition.z >= 30F && controllerPosition.z < 210F) || (!rightHand && controllerPosition.z >= 150F && controllerPosition.z < 330F))
            {
                currentWeaponMode = MODE1;
            }
            else
            {
                currentWeaponMode = MODE2;
            }
        }

        // Déclenchement de l'animation de la main droite
        if (previousWeaponMode != currentWeaponMode && rightHand)
        {
            //Debug.Log("changement de mode");
            GameObject ironMan = GameObject.Find("/IronMan");
            AnimationController animation = ironMan.GetComponent<AnimationController>();
            animation.CloseYourHand(currentWeaponMode == MODE2);
        }
        
        // Activation/désactivation des super armes
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            superWeaponIsEnabled = !superWeaponIsEnabled;
        }

        // test : on affiche le placeholder du laser gun
        laserCube.GetComponent<Renderer>().enabled = Convert.ToBoolean(currentWeaponMode);
        #endregion


        // On gère le tir uniquement si le vol n'est pas activé
        if (!Controller.GetPress(SteamVR_Controller.ButtonMask.Grip))
        {
            // cas où l'on arrête de tirer
            if ((!Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger) || currentWeaponMode != MODE1 || superWeaponIsEnabled != superWeaponWasEnabled) && weaponState == FIRING && superWeaponWasEnabled)
            {
                HoldYourFire();
            }
            // cas où l'on annule un tir en cours de chargement
            if ((!Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger) || currentWeaponMode != MODE1 || superWeaponIsEnabled != superWeaponWasEnabled) && (weaponState == LOADING || weaponState == LOADED))
            {
                AbortFiring();
            }

            // beam cannon
            if (currentWeaponMode == MODE1 && superWeaponIsEnabled && Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger))
            {
                // on commence à tirer
                if (weaponState == WAITING)
                {
                    UpYourBeam();
                }
                // moment où l'arme est prête et que l'on continue de tirer
                if (weaponState == LOADED)
                {
                    source.clip = beamFiring;
                    source.Play();
                    weaponState = FIRING;
                }
                // tir en cours
                else if (weaponState == FIRING)
                {
                    ShowLaser();
                }
            }

            // repulse gun
            if (currentWeaponMode == MODE1 && !superWeaponIsEnabled && Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger))
            {
                // on commence à tirer
                if (weaponState == WAITING)
                {
                    UpYourRepulse();
                }
                // moment où l'arme est prête et que l'on continue de tirer
                if (weaponState == LOADED)
                {
                    source.PlayOneShot(repulseFiring, 8F);

                    StartCoroutine(WaitUntilRepulseHasFired());
                }
            }
            // le repulse gun continue de tirer même si on lâche la gâchette
            if (currentWeaponMode == MODE1 && !superWeaponIsEnabled)
            {
                // tir en cours
                if (weaponState == FIRING && smallFireTest)
                {
                    ShowLaser();
                }
            }

            // tir lasers
            if (currentWeaponMode == MODE2 && Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger) && freeFire)
            {
                //Debug.Log("Tir mode 2");
                StartCoroutine(WaitForFireRate());

                GameObject throwThis = Instantiate(projectile, laserCube.position, laserCube.rotation) as GameObject;
                throwThis.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 4000F);

                source.PlayOneShot(laserSound, 1F);
            }
        }
        // cas où l'on enclenche le vol lors d'un tir en cours => on stoppe le tir
        else if ((weaponState == FIRING && superWeaponWasEnabled) || weaponState == LOADING || weaponState == LOADED)
        {
            HoldYourFire();
        }
    }
}
