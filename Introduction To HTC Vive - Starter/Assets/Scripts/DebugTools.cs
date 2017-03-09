using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTools : MonoBehaviour {
    private SteamVR_TrackedObject trackedObj;
    public Transform head;

    private Vector3 relativeDirection;
    private int mode;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        relativeDirection = trackedObj.transform.rotation * (head.position - trackedObj.transform.position);

        if (relativeDirection.y >= 0)
        {
            mode = 1;
        }
        else
        {
            mode = 2;
        }

        //if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        //{
        //    Vector3 relativeDirection = trackedObj.transform.rotation * (head.position - trackedObj.transform.position);
        //    //Debug.Log(head.position - trackedObj.transform.position);
        //    //Debug.Log(relativeDirection);
        //    Debug.Log("rotation = " + transform.localRotation.ToString());
        //    Debug.Log("position = " + transform.localPosition.ToString());
        //}
    }
}
