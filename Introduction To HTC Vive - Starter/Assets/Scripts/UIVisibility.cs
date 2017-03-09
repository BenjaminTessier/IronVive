using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVisibility : MonoBehaviour {
    public SteamVR_TrackedObject leftTrackedObj;
    public SteamVR_TrackedObject rightTrackedObj;

    private SteamVR_Controller.Device leftController
    {
        get { return SteamVR_Controller.Input((int)leftTrackedObj.index); }
    }

    private SteamVR_Controller.Device rightController
    {
        get { return SteamVR_Controller.Input((int)rightTrackedObj.index); }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (leftController.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu) || rightController.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            GameObject child = gameObject.transform.FindChild("HUD").gameObject;
            child.SetActive(!child.activeSelf);
        }

    }
}
