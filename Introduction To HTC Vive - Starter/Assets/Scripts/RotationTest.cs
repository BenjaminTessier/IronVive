using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour {

    public SteamVR_TrackedObject trackedObj;
    public Transform head;
    //public Transform controller;

    private Quaternion rotation;
    private Vector3 vector;
    //private double distance;

    private SteamVR_Controller.Device main
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        rotation = Quaternion.FromToRotation(transform.InverseTransformVector(head.up), transform.InverseTransformVector(trackedObj.transform.up));
        vector = rotation.eulerAngles;
        //Debug.DrawRay(trackedObj.transform.position, trackedObj.transform.up, Color.red);

        if (main.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("Rotation : " + rotation.eulerAngles.ToString());
        }

        //Vector3 controllerPosition = trackedObj.transform.position;
        //Vector3 cubePosition = cube.position;
        //vector = cube.rotation * (cube.position - trackedObj.transform.position);
        //distance = Math.Sqrt(Math.Pow(controllerPosition.x - cubePosition.x, 2) + Math.Pow(controllerPosition.y - cubePosition.y, 2) + Math.Pow(controllerPosition.z - cubePosition.z, 2));
    }
}
