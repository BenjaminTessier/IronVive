using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
    public AudioClip[] clips;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (!source.isPlaying)
        {
            int clipNum = Random.Range(0, clips.Length);
            source.PlayOneShot(clips[clipNum], 1F);
        }
	}
}
