using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour {

    private const int MODE1 = 0;
    private const int MODE2 = 1;
    private const int MODE3 = 2;
    private const int MODE4 = 3;

    private int armMode;

    private SteamVR_TrackedObject trackedObj;
    // 1
    public GameObject laserPrefab;
    // 2
    private GameObject laser;
    // 3
    private Transform laserTransform;
    // 4
    private Vector3 hitPoint;

    public AudioClip loadingRepulseGun;
    private AudioSource source;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private void ShowLaser(RaycastHit hit)
    {
        Transform originRepulseGun = trackedObj.transform.FindChild("invisibleCube");

        // 1
        laser.SetActive(true);
        // 2
        laserTransform.position = Vector3.Lerp(originRepulseGun.position, hitPoint, .5f);
        // 3
        laserTransform.LookAt(hitPoint);
        // 4
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y,
            hit.distance);
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        source = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
        // 1
        laser = Instantiate(laserPrefab);
        // 2
        laserTransform = laser.transform;

        armMode = MODE1;
    }
	
	// Update is called once per frame
	void Update () {

        if (armMode == MODE1 && Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log(trackedObj.transform.rotation.ToString());
            Debug.Log(trackedObj.transform.localRotation.ToString());
            //trackedObj.transform.parent.TransformDirection
        }

        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Grip) && Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 test = Controller.GetAxis();
            if (test.x <= 0 && test.y >= 0 && armMode != MODE1)
            {
                armMode = MODE1;
                Debug.Log("haut gauche");
            }
            else if (test.x > 0 && test.y >= 0 && armMode != MODE2)
            {
                armMode = MODE2;
                source.PlayOneShot(loadingRepulseGun, 1F);
                Debug.Log("haut droit");
            }
            else if (test.x < 0 && test.y < 0 && armMode != MODE3)
            {
                armMode = MODE3;
                Debug.Log("bas gauche");
            }
            else if (test.x > 0 && test.y < 0 && armMode != MODE4)
            {
                armMode = MODE4;
                Debug.Log("bas droit");
            }
        }

        if (armMode == MODE2 && Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            RaycastHit hit;
            Vector3 distanceToArm = new Vector3(1.5F, 0, 0);
            Vector3 positionToArm = transform.rotation * distanceToArm;
            Transform originRepulseGun = trackedObj.transform.FindChild("invisibleCube");

            Debug.Log(transform.position.ToString() + " = " + trackedObj.transform.position.ToString());

            //originRepulseGun.position = new Vector3(0, 0, -1);
            // objets rayCastIgnore
            int layerMask = 1 << 2;
            layerMask = ~layerMask;
            //if (Physics.Raycast(trackedObj.transform.position + positionToArm, transform.TransformDirection(new Vector3(0, -1, 0.2F)), out hit, 100, layerMask))
            if (Physics.Raycast(originRepulseGun.position, originRepulseGun.TransformDirection(new Vector3(0, -1, 0.05F)), out hit, 100, layerMask))
            {
                hitPoint = hit.point;
                ShowLaser(hit);
            }
        }
        else
        {
            laser.SetActive(false);
        }
    }
}
