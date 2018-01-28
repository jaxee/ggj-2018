using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Kill : MonoBehaviour {
	ParticleSystem p;
	// Use this for initialization
	void Start () {
		p = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (p.isPlaying)
			return;
		Destroy (transform.root.gameObject);
	}
}
