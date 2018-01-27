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
	bool isDoor;

	void Start(){
		meshRender = GetComponent<MeshRenderer> ();
		anim = GetComponent<Animator> ();
		meshObs = GetComponent<NavMeshObstacle> ();
		manager = GameObject.FindObjectOfType<Manager> ();
		if (tag == "Door") {
			isDoor = true;
		} else {
			isDoor = false;
		}
	}

	void OnMouseEnter(){
		meshRender.enabled = true;
	}

	void OnMouseExit(){
		meshRender.enabled = false;
	}

	void OnMouseDown(){
		Trigger ();
	}

	public void Trigger(){
		if(meshObs != null)
			meshObs.enabled = !meshObs.enabled;
		
		anim.SetTrigger ("toggle");

		if (isDoor) {
			if (!Manager.doors.Contains (this)) {
				Manager.doors.Add (this);
			} else {
				Manager.doors.Remove (this);
			}
		}

	}

}
