using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScore : MonoBehaviour {
    
    GameObject scoreManagement;
    Score scoreScript;
    Text textObject;

    private void Awake()
    {
        scoreManagement = GameObject.Find("/Score");
        scoreScript = scoreManagement.GetComponent<Score>();
        textObject = GetComponent<Text>();
    }

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
        textObject.text = "Score : " + scoreScript.score.ToString();
    }
}
