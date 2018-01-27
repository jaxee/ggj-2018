using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

	public int typeOfRoom; // 1 is normal, 2 is hazard, 3 is escape
	public int numberOfDoors;
	private bool[] doorStatus;
	public List<Transform> loiterNodes = new List<Transform>();
	public List<GameObject> playersInRoom = new List<GameObject>();

	float virusSpreadTime = 5f;
	bool spreadingVirus = false;
	
	void Update () {
		// Keep track of the number of healthy and sick people in a room
	}

	public void createPeople(int healthy, int sick) {
		for (int i = 0; i < healthy; i++) {
			// Create healthy people
		}

		for (int j = 0; j < sick; j++) {
			// Create sick people
		}

		if (!spreadingVirus) {
			StartCoroutine (InfectPeople());
		}
			
	}

	public int getNumberOfDoors () {
		return numberOfDoors;
	}

	void OnTriggerEnter(Collider c){
		if (c.tag == "Player") {
			AIComponent player = c.GetComponent<AIComponent> ();
			if (!player.currentRoom == gameObject) {
				playersInRoom.Add (c.gameObject);
				c.GetComponent<AIComponent> ().currentRoom = gameObject;
				Debug.Log ("Player " + c.name + " entered " + name);
			}
		}
	}

	void OnTriggerExit(Collider c){
		if (c.tag == "Player") {
			playersInRoom.Remove (c.gameObject);
		}
	}

	IEnumerator InfectPeople()
	{
		spreadingVirus = true;
		yield return new WaitForSeconds (virusSpreadTime);
		Debug.Log ("DIE DIE DIE!");
		foreach (GameObject player in playersInRoom) {
			AIComponent p = player.GetComponent<AIComponent> ();

		}
		spreadingVirus = false;
	}
}
