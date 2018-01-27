using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class Interact : MonoBehaviour {
	MeshRenderer meshRender;
	Animator anim;
	NavMeshObstacle meshObs;

	void Start(){
		meshRender = GetComponent<MeshRenderer> ();
		anim = GetComponent<Animator> ();
		meshObs = GetComponent<NavMeshObstacle> ();
	}

	void OnMouseEnter(){
		meshRender.enabled = true;
	}

	void OnMouseExit(){
		meshRender.enabled = false;
	}

	void OnMouseDown(){
		meshObs.enabled = !meshObs.enabled;
		anim.SetTrigger ("door_toggle");
	}
}
