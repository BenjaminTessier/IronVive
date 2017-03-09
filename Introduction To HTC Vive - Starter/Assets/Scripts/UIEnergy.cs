using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnergy : MonoBehaviour
{
    GameObject energyManagement;
    Fly energyScript;
    Text textObject;

    private void Awake()
    {
        energyManagement = GameObject.Find("/[CameraRig]");
        energyScript = energyManagement.GetComponent<Fly>();
        textObject = GetComponent<Text>();
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        textObject.text = "Energy : " + ((int)energyScript.energy / 100).ToString();
    }
}
