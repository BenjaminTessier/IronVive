using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {
    Animator anim;
    int closeAnimation = Animator.StringToHash("close");

    public void CloseYourHand(bool close)
    {
        anim.SetBool(closeAnimation, close);
    }

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
    }
}
