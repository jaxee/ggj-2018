using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Kill : MonoBehaviour {
	AudioSource a;
	// Use this for initialization
	void Start () {
		a = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (a.isPlaying)
			return;
		Destroy (transform.root.gameObject);
	}
}
