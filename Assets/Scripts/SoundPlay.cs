using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlay : MonoBehaviour {
	public AudioSource m_Audiosource;
	public AudioEvent m_AudioEvent;

	public bool playOnStart = true;

	void Awake(){
		if (m_Audiosource == null) {
			m_Audiosource = GetComponent<AudioSource> ();
		}
	}

	// Use this for initialization
	void Start () {
		if (playOnStart) {
			m_AudioEvent.Play (m_Audiosource);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Play (){
		m_AudioEvent.Play (m_Audiosource);
	}
}
