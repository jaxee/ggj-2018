using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour {
	public bool interactable = true;
	public MeshRenderer meshRender;
	Animator anim;
	public Manager manager;
	// Use this for initialization
	public virtual void Start () {
		meshRender = GetComponent<MeshRenderer> ();
		anim = GetComponent<Animator> ();
		manager = GameObject.FindObjectOfType<Manager> ();
	}

	public virtual void OnMouseEnter(){
		if (interactable) {
			meshRender.enabled = true;
		}
	}

	public virtual void OnMouseExit(){
		meshRender.enabled = false;
	}

	public virtual void OnMouseDown(){
		if (interactable)
			Trigger ();
	}

	public virtual void Trigger(bool ignoreMaxDoors = false){
		anim.SetTrigger ("toggle");
	}
}
