using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class Interact : MonoBehaviour {
	MeshRenderer meshRender;
	Animator anim;
	NavMeshObstacle meshObs;
	Manager manager;

	void Start(){
		meshRender = GetComponent<MeshRenderer> ();
		anim = GetComponent<Animator> ();
		meshObs = GetComponent<NavMeshObstacle> ();
		manager = GameObject.FindObjectOfType<Manager> ();
	}

	void OnMouseEnter(){
		meshRender.enabled = true;
	}

	void OnMouseExit(){
		meshRender.enabled = false;
	}

	void OnMouseDown(){
		TriggerDoor ();
	}

	public void TriggerDoor(){
		meshObs.enabled = !meshObs.enabled;
		anim.SetTrigger ("door_toggle");
		if (!Manager.doors.Contains (this)) {
			Manager.doors.Add (this);
		} else {
			Manager.doors.Remove (this);
		}

	}
}
