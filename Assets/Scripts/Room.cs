using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

	public int typeOfRoom; // 1 is normal, 2 is hazard, 3 is escape
	private bool[] doorStatus;
	public List<GameObject> doors = new List<GameObject>();
	public List<Transform> loiterNodes = new List<Transform>();
	public List<GameObject> playersInRoom = new List<GameObject>();

	float virusSpreadTime = 4f;
	bool spreadingVirus = false;
	int numOfInfectedPlayers = 0;

	void Update () {
		if (!spreadingVirus) {
			StartCoroutine (InfectPeople ());
		}
	}

	void Start() {
	}

	void OnTriggerEnter(Collider c){
		if (c.tag == "Player") {
			AIComponent player = c.GetComponent<AIComponent> ();
			if (!playersInRoom.Contains(c.gameObject)) {
				playersInRoom.Add (c.gameObject);
				if (player.isInfected)
					numOfInfectedPlayers++;
				c.GetComponent<AIComponent> ().currentRoom = gameObject;
			}
		}
	}

	void OnTriggerExit(Collider c){
		if (c.tag == "Player") {
			AIComponent player = c.GetComponent<AIComponent> ();
			playersInRoom.Remove (c.gameObject);
			if (player.isInfected)
				numOfInfectedPlayers--;
		}
	}

	IEnumerator InfectPeople()
	{
		spreadingVirus = true;
		yield return new WaitForSeconds (virusSpreadTime);
		if (numOfInfectedPlayers > 0) {
			foreach (GameObject player in playersInRoom) {
				AIComponent p = player.GetComponent<AIComponent> ();
				//Resilience is their %chance to get infected.
				if (Random.Range (0, 100) > p.resilience && !p.isInfected) {
					p.isInfected = true;
				} else {
					p.resilience -= numOfInfectedPlayers * 2;
					p.resilience = Mathf.Clamp (p.resilience, 0, 100);
				}
			}
		}
		spreadingVirus = false;
	}
}
