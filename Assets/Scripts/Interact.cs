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

	//You'll float too
	float airlockCD = 15f;
	float airlockTimer;
	bool airlockOpen = false;

	float scannerCD = 30f;
	bool canScan = true;

	bool interactable = true;

	// Escape pod variables
	bool isEscapePod;
	bool isEscapePodBoarding;
	static System.Random escapePodEntrance = new System.Random();
	public List<GameObject> evacuationPoints = new List<GameObject>();
	bool leavingEmpty= false;

	public ParticleSystem[] scan;
	public GameObject sick;

	int negScoreDiff;
	int posScoreDiff;

	public GameObject buttonObject;

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
		} else if (tag == "Airlock") {
			isAirLock = true;
		} else
			isScanner = true;
		oldColor = meshRender.material.color;
	}

	void OnMouseEnter(){
		if(interactable)
		{
		meshRender.enabled = true;
			if (Manager.doors.Count == manager.maxDoorsClosed) {
				Manager.doors [0].meshRender.enabled = true;
				Manager.doors [0].meshRender.material.color = new Color (1f, 0f, 0f, 0.5f);
			}
		}
	}

	void OnMouseExit(){
		if (interactable) {
			meshRender.enabled = false;
			if (Manager.doors.Count >= manager.maxDoorsClosed) {
				Manager.doors [0].meshRender.enabled = false;
				Manager.doors [0].meshRender.material.color = oldColor;
			}
		}
	}

	void OnMouseDown(){
		if (interactable)
			Trigger ();
	}

	public void Trigger(){
		posScoreDiff = 0;
		negScoreDiff = 0;

		if (meshObs != null) {
			meshObs.enabled = !meshObs.enabled;
			if (meshObs.enabled) {
				GetComponent<SoundPlayManager> ().PlayOne ();
			} else {
				GetComponent<SoundPlayManager> ().PlayTwo ();
			}
		} else {
			Instantiate (buttonObject, transform.position, Quaternion.identity);
		}

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

			if (this.meshObs.enabled) {
				manager.OpenDoor ();
			} else {
				manager.CloseDoor ();
			}
		}
			
		if (isEscapePod) {
			isEscapePodBoarding = true;
			if (leavingEmpty) {
				anim.SetTrigger ("toggle");
				GetComponentInChildren<AudioSource> ().Play (54100);
				manager.EscapePodLaunched (posScoreDiff, negScoreDiff);
			}
			currentRoom = transform.parent.GetComponent<Room>();
			foreach (GameObject door in currentRoom.doors) {
				Interact d = door.GetComponent<Interact> ();
				if (!d.meshObs.enabled) {
					d.anim.SetTrigger ("toggle");
					d.meshObs.enabled = true;

					//Destroy (d);
				}
				Manager.doors.Remove (d);
				d.interactable = false;
			}

			if (currentRoom.playersInRoom.Count == 0) {
				
				leavingEmpty = true;
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

		if (isAirLock) {
			StartCoroutine (DoAirlockStuff ());
		}

		if (isScanner && canScan) {
			StartCoroutine (DoScannerStuff());
			canScan = false;
		}

	}

	public void OnTriggerStay(Collider c){

		if (c.tag == "Player" && isEscapePodBoarding) {
			//Collect players, do points and destroy player game object
			Debug.Log("Collect player: " + c.name);

			AIComponent player = c.GetComponent<AIComponent>();
			if (player.isInfected) {
				negScoreDiff += manager.removePoints ();
				manager.numberOfSickPeople--;
			} else {
				manager.addPoints ();
				posScoreDiff += manager.addPoints ();
				manager.numberOfHealthyPeople--;
			}
			manager.numberOfPeople--;

			player.destoryPlayer();

			if (currentRoom.playersInRoom.Count == 0) {
				anim.SetTrigger ("toggle");
				GetComponentInChildren<AudioSource> ().Play (54100);;
				manager.EscapePodLaunched (posScoreDiff, negScoreDiff);
			}
		}
	}


	IEnumerator DoAirlockStuff(){
		yield return new WaitForSeconds (1f);
		airlockTimer = Time.time;
		airlockOpen = true;
		SealDoors ();
		transform.root.GetComponentInChildren<ParticleSystem> ().Play ();
		transform.parent.GetComponentInChildren<AudioSource> ().Play ();
		foreach (GameObject player in currentRoom.playersInRoom) {
			AIComponent p = player.GetComponent<AIComponent> ();
			Destroy (p.meshAgent);
			p.pull = true;
			Rigidbody rb = p.GetComponent<Rigidbody> ();
			rb.constraints = RigidbodyConstraints.None;
			rb.useGravity = false;
			rb.drag = 0f;
			rb.angularDrag = 0f;
		}
	}

	void SealDoors(){
		currentRoom = transform.root.GetComponent<Room> ();
		foreach (GameObject door in currentRoom.doors) {
			Interact d = door.GetComponent<Interact> ();
			if (!d.meshObs.enabled) {
				d.anim.SetTrigger ("toggle");
				d.meshObs.enabled = true;
			}
			d.interactable = false;
		}
	}

	IEnumerator DoScannerStuff(){
		SealDoors ();
		yield return new WaitForSeconds (1.5f);
		GetComponentInChildren<AudioSource> ().Play ();
		scan [0].Play ();
		scan [1].Play ();
		foreach (GameObject player in currentRoom.playersInRoom) {
			AIComponent ai = player.GetComponent<AIComponent> ();
			if (ai.isInfected) {
				Instantiate (sick, ai.transform.position + Vector3.up * 4, Quaternion.identity, ai.transform);
			}
		}
		StartCoroutine (UnlockDoors ());
		StartCoroutine (ScannerCooldown ());
	}

	IEnumerator UnlockDoors(){
		yield return new WaitForSeconds (1.5f);
		foreach (GameObject door in currentRoom.doors) {
			Interact d = door.GetComponent<Interact> ();
			d.interactable = true;
		}
	}

	IEnumerator ScannerCooldown(){
		interactable = false;
		yield return new WaitForSeconds (scannerCD);
		interactable = true;
		canScan = true;
		anim.SetTrigger ("toggle");
	}
}
