using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour {

	//List of rooms to pick from for destination
	public List<GameObject> rooms = new List<GameObject>();

	public enum State{
		Loiter,
		Roam,
		Panic,
		Evacuate,
		MAX_STATES
	};

	public Transform SetRoamDestination(GameObject currentRoom){
		Transform dest;
		do {
			dest = rooms [Random.Range (0, rooms.Count)].transform;
		} while(dest == currentRoom.transform);
		return dest;
	}

	public Transform SetLoiterDestination(GameObject currentRoom){
		Transform dest;
		do {
			dest = rooms [Random.Range (0, rooms.Count)].transform;
		} while(dest == currentRoom.transform);
		return dest;
	}

	public Transform Decide(GameObject currentRoom, int roamChance){
		if (Random.Range (0, 100) <= roamChance) {
			return SetRoamDestination(currentRoom);
		} else {
			return State.Loiter;
		}
	}
}
