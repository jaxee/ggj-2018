using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour {

	//List of rooms to pick from for destination
	private GameObject[] roomsGO;
	public static List<Room> rooms = new List<Room>();


	public enum State{
		Loiter,
		Roam,
		Panic,
		Evacuate,
		MAX_STATES
	};

	void Start()
	{
		roomsGO = GameObject.FindGameObjectsWithTag ("Room");

		for (int i = 0; i < roomsGO.Length; i++) {
			rooms.Add(roomsGO[i].GetComponent<Room>());
		}
	}

	public Transform SetRoamDestination(GameObject currentRoom){
		Room destRoom;

		//Select a room at random from our list
		do{
			destRoom = rooms [Random.Range (0, rooms.Count)];
		} while(destRoom.gameObject == currentRoom);

		//Return a random transform node in the selected room
		return SetLoiterDestination(destRoom.gameObject);
	}

	public Transform SetLoiterDestination(GameObject targetRoom){
		Room room = targetRoom.GetComponent<Room> ();
		return room.loiterNodes[Random.Range (0, room.loiterNodes.Count)];
	}

	public Transform Decide(GameObject currentRoom, int roamChance){
		if (Random.Range (0, 100) <= roamChance) {
			return SetRoamDestination(currentRoom);
		} else {
			return SetLoiterDestination(currentRoom);
		}
	}
}
