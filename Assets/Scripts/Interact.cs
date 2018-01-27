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
	Color oldColor;
	bool isDoor;
	bool isAirLock;
	bool isScanner;

	// Escape pod variables
	bool isEscapePod;
	bool isEscapePodBoarding;
	static System.Random escapePodEntrance = new System.Random();
	public List<GameObject> evacuationPoints = new List<GameObject>();

	void Start(){
		meshRender = GetComponent<MeshRenderer> ();
		anim = GetComponent<Animator> ();
		meshObs = GetComponent<NavMeshObstacle> ();
		manager = GameObject.FindObjectOfType<Manager> ();

		isEscapePodBoarding = false;

		if (tag == "Door") {
			isDoor = true;
		} else if (tag == "Escape") {
			isEscapePod = true;
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
			
		if (isEscapePod) {
			isEscapePodBoarding = true;

			currentRoom = transform.parent.GetComponent<Room>();
			foreach (GameObject door in currentRoom.doors) {
				Interact d = door.GetComponent<Interact> ();
				if (!d.meshObs.enabled) {
					d.anim.SetTrigger ("toggle");
					d.meshObs.enabled = true;
					Destroy (d);
				}
			}

			if (currentRoom.playersInRoom.Count == 0) {
				Debug.Log ("LAUNCH SHIP ANIMATION");
			} else {
				foreach(GameObject player in currentRoom.playersInRoom) {
					AIComponent p = player.GetComponent<AIComponent> ();
					int r = escapePodEntrance.Next(evacuationPoints.Count);
					Debug.Log ("Go to entrance: " + evacuationPoints[r]);
					p.meshAgent.SetDestination (evacuationPoints[r].transform.position);
					p.meshAgent.isStopped = false;
					p.meshAgent.speed = 9f;
					p.isBoarding = true;

				}
			}
		}

	}

	public void OnTriggerEnter(Collider c){
		if (c.tag == "Player" && isEscapePodBoarding) {
			//Collect players, do points and destroy player game object
			Debug.Log("Collect player: " + c.name);
			c.GetComponent<AIComponent>().destoryPlayer();

			if (currentRoom.playersInRoom.Count == 0) {
				Debug.Log ("LAUNCH SHIP ANIMATION");
			}
		}
	}
}
