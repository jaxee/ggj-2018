using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour {

	//List of rooms to pick from for destination
	private GameObject[] roomsGO;
	public static List<Room> rooms = new List<Room>();
	AIComponent ai;

	public enum State{
		Loiter,
		Roam,
		Panic,
		Evacuate,
		MAX_STATES
	};

	void Start()
	{
		ai = GetComponent<AIComponent> ();
		roomsGO = GameObject.FindGameObjectsWithTag ("Room");

		for (int i = 0; i < roomsGO.Length; i++) {
			rooms.Add(roomsGO[i].GetComponent<Room>());
		}
	}

	public Transform SetRoamDestination(GameObject currentRoom){
		Room destRoom;
		Room currRoom = currentRoom.GetComponent<Room> ();
		//Select a room at random from our list
		if (currRoom.connectedRooms.Count > 0) {
			destRoom = currRoom.connectedRooms [Random.Range (0, currRoom.connectedRooms.Count)];

			//Return a random transform node in the selected room
			return SetLoiterDestination (destRoom.gameObject);
		}
		return SetLoiterDestination (currentRoom);
	}

	public Transform SetLoiterDestination(GameObject targetRoom){
		
		Room room = targetRoom.GetComponent<Room> ();
		Transform dest = room.loiterNodes[Random.Range (0, room.loiterNodes.Count)];
		UnityEngine.AI.NavMeshPath p = new UnityEngine.AI.NavMeshPath();
		if (ai.meshAgent.CalculatePath (dest.position, p) && p.status != UnityEngine.AI.NavMeshPathStatus.PathInvalid)
			return dest;
		else 
			return SetLoiterDestination (ai.currentRoom);
	}

	public Transform Decide(GameObject currentRoom, int roamChance){
		if (Random.Range (0, 100) <= roamChance) {
			return SetRoamDestination(currentRoom);
		} else {
			return SetLoiterDestination(currentRoom);
		}
	}
}
