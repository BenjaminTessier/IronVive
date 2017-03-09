using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyScript : MonoBehaviour {

    public Transform head;
    public Transform body;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //body.position = new Vector3(head.position.x, (head.position.y - 0.65F), head.position.z + 0.25F);
        body.position = new Vector3(head.position.x, (head.position.y - 2.30F), head.position.z) + head.forward * (-0.3F);
        body.rotation = Quaternion.Euler(new Vector3(0, head.rotation.eulerAngles.y, 0));
	}
}
