using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayManager : MonoBehaviour {

	[SerializeField]
	public SoundPlay[] sounds;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayOne(){
		sounds [0].Play ();
	}

	public void PlayTwo(){
		sounds [1].Play ();
	}
}
