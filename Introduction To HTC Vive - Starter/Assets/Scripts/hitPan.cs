using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitPan : MonoBehaviour {

    public AudioClip panSound;
    private AudioSource source;

	// Use this for initialization
	void Awake() {
        source = GetComponent<AudioSource>();
	}
	
    void OnCollisionEnter(Collision coll)
    {
        source.PlayOneShot(panSound, 1F);
    }
}
