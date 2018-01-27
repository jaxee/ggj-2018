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
	Color oldColor;

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
		oldColor = meshRender.material.color;
	}

	void OnMouseEnter(){
		meshRender.enabled = true;
		if (Manager.doors.Count == manager.maxDoorsClosed) {
			Manager.doors [0].meshRender.enabled = true;
			Manager.doors [0].meshRender.material.color = new Color (1f, 0f, 0f, 0.5f);
		}
	}

	void OnMouseExit(){
		meshRender.enabled = false;
		if (Manager.doors.Count >= manager.maxDoorsClosed) {
			Manager.doors [0].meshRender.enabled = false;
			Manager.doors [0].meshRender.material.color = oldColor;
		}
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
			if (Manager.doors.Count >= manager.maxDoorsClosed) {
				Manager.doors [0].meshRender.enabled = false;
				Manager.doors [0].meshRender.material.color = oldColor;
			}
		}

	}

}
