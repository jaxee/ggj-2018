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

	public ParticleSystem[] scan;
	public GameObject sick;


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
		} else if (tag == "Airlock")
			isAirLock = true;
		else
			isScanner = true;
		oldColor = meshRender.material.color;
	}

	void OnMouseEnter(){
		if (interactable) {
			meshRender.enabled = true;
			if (Manager.doors.Count == manager.maxDoorsClosed && isDoor) {
				Manager.doors [0].meshRender.enabled = true;
				Manager.doors [0].meshRender.material.color = new Color (1f, 0f, 0f, 0.5f);
			}
		}
	}

	void OnMouseExit(){
		meshRender.enabled = false;
		if (Manager.doors.Count >= manager.maxDoorsClosed && isDoor) {
			Manager.doors [0].meshRender.enabled = false;
			Manager.doors [0].meshRender.material.color = oldColor;
		}
	}

	void OnMouseDown(){
		if(interactable)
			Trigger ();
	}

	public void Trigger(){
		if (meshObs != null)
			meshObs.enabled = !meshObs.enabled;

		if(!(isScanner && !canScan))
			anim.SetTrigger ("toggle");

		if (isDoor) {
			if (meshObs.enabled) {
				this.GetComponent<SoundPlayManager> ().PlayOne ();
			} else {
				this.GetComponent<SoundPlayManager> ().PlayTwo ();
			}
				

			if (!Manager.doors.Contains (this)) {
				Manager.doors.Add (this);
			} else {
				Manager.doors.Remove (this);
			}
			if (Manager.doors.Count >= manager.maxDoorsClosed && isDoor) {
				Manager.doors [0].meshRender.enabled = false;
				Manager.doors [0].meshRender.material.color = oldColor;
			}
		}
		
		if (isEscapePod) {
			isEscapePodBoarding = true;

			currentRoom = transform.parent.GetComponent<Room> ();
			foreach (GameObject door in currentRoom.doors) {
				Interact d = door.GetComponent<Interact> ();
				if (!d.meshObs.enabled) {
					d.anim.SetTrigger ("toggle");
					d.meshObs.enabled = true;
					Manager.doors.Remove (d);
					Destroy (d);
				}
			}

			if (currentRoom.playersInRoom.Count == 0) {
				Debug.Log ("LAUNCH SHIP ANIMATION");
			} else {
				foreach (GameObject player in currentRoom.playersInRoom) {
					AIComponent p = player.GetComponent<AIComponent> ();
					int r = escapePodEntrance.Next (evacuationPoints.Count);
					Debug.Log ("Go to entrance: " + evacuationPoints [r]);
					p.meshAgent.SetDestination (evacuationPoints [r].transform.position);
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

	void Update(){
		//If the airlock is currently open, the cooldown has elapsed, and the room is empty, reset.
		if (airlockOpen && Time.time - airlockTimer > airlockCD && transform.root.GetComponent<Room>().playersInRoom.Count == 0) {
			anim.SetTrigger ("toggle");
			airlockOpen = false;
			foreach (GameObject door in currentRoom.doors) {
				Interact d = door.GetComponent<Interact> ();
				d.interactable = true;
				d.anim.SetTrigger ("toggle");
				d.meshObs.enabled = false;
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

	IEnumerator DoAirlockStuff(){
		yield return new WaitForSeconds (1f);
		airlockTimer = Time.time;
		airlockOpen = true;
		SealDoors ();
		transform.root.GetComponentInChildren<ParticleSystem> ().Play ();
		transform.root.GetComponentInChildren<ParticleSystem> ().gameObject.GetComponent<AudioSource> ().Play ();
		Debug.Log (transform.root.GetComponentInChildren<AudioSource> ().clip);
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
				d.interactable = false;
			}
		}
	}

	IEnumerator DoScannerStuff(){
		SealDoors ();
		yield return new WaitForSeconds (1.5f);
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
		yield return new WaitForSeconds (scannerCD);
		canScan = true;
		anim.SetTrigger ("toggle");
	}
}
