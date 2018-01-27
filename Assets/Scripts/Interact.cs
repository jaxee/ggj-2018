using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class Interact : MonoBehaviour {
	MeshRenderer meshRender;
	public Animator anim;
	public NavMeshObstacle meshObs;
	Manager manager;
	Room currentRoom;
	bool isDoor;
	bool isEscapePod;
	bool isAirLock;
	bool isScanner;

	void Start(){
		meshRender = GetComponent<MeshRenderer> ();
		anim = GetComponent<Animator> ();
		meshObs = GetComponent<NavMeshObstacle> ();
		manager = GameObject.FindObjectOfType<Manager> ();

		Debug.Log ("Tag: " + tag);

		if (tag == "Door") {
			isDoor = true;
		} else if (tag == "Escape") {
			isEscapePod = true;
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

		if (isEscapePod) {
			currentRoom = transform.parent.GetComponent<Room>();
			foreach (GameObject door in currentRoom.doors) {
				Interact d = door.GetComponent<Interact> ();
				if (!d.meshObs.enabled) {
					d.anim.SetTrigger ("toggle");
					d.meshObs.enabled = true;
				}
			}

			if (!currentRoom.playersInRoom.Count == 0) {
				Debug.Log ("Players: " + currentRoom.playersInRoom);
			} else {
				Debug.Log ("LAUNCH SHIP ANIMATION");
			}

		}

	}
}
