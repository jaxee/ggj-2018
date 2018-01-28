using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : Highlight {

	public NavMeshObstacle meshObs;

	public bool isOpen = true;

	Color oldColor;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		meshObs.GetComponent<NavMeshObstacle> ();
		oldColor = meshRender.material.color;
	}

	public override void OnMouseEnter(){
		base.OnMouseEnter ();
		if (Manager.doors.Count == manager.maxDoorsClosed) {
			for (int i = 0; i < Manager.doors.Count; i++) {
				if (Manager.doors [i].interactable) {
					Manager.doors [i].meshRender.enabled = true;
					Manager.doors [i].meshRender.material.color = new Color (1f, 0f, 0f, 0.5f);
					break;
				}
			}
		}
	}

	public override void OnMouseExit(){
		base.OnMouseExit ();
		if (Manager.doors.Count >= manager.maxDoorsClosed) {
			for (int i = 0; i < Manager.doors.Count; i++) {
				if (Manager.doors [i] != this) {
					Manager.doors [i].meshRender.enabled = false;
					Manager.doors [i].meshRender.material.color = oldColor;
					break;
				}
			}
		}
	}

	public override void OnMouseDown(){
		base.OnMouseDown ();
	}

	public override void Trigger(bool ignoreMaxDoors = false){
		base.Trigger ();
		isOpen = !isOpen;
		meshObs.enabled = !meshObs.enabled;

		if (!ignoreMaxDoors) {
		if (!Manager.doors.Contains (this) && isOpen) {
			Manager.doors.Add (this);
		} else if(!isOpen) {
			Manager.doors.Remove (this);
		}

			for (int i = 0; i < Manager.doors.Count; i++) {
				if (Manager.doors [i] != this) {
					Manager.doors [i].meshRender.enabled = false;
					Manager.doors [i].meshRender.material.color = oldColor;
					break;
				}
			}
		}

		if (isOpen) {
			manager.OpenDoor ();
		} else {
			manager.CloseDoor ();
		}
	}
}
