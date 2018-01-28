using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour {

	//List of rooms to pick from for destination
	private GameObject[] roomsGO;
	public static List<Room> rooms = new List<Room>();
	AIComponent ai;
	Manager manager;
	public bool panic = false;

	void Start()
	{
		manager = GameObject.FindObjectOfType<Manager> ();
		ai = GetComponent<AIComponent> ();
		roomsGO = GameObject.FindGameObjectsWithTag ("Room");

		for (int i = 0; i < roomsGO.Length; i++) {
			rooms.Add(roomsGO[i].GetComponent<Room>());
		}
	}

	public Transform SetRoamDestination(GameObject currentRoom){
		Room currRoom = currentRoom.GetComponent<Room> ();
		//Select a room at random from our list
		List<Room> adjacentOpenRooms = new List<Room>();
		foreach (GameObject door in currRoom.doors) {
			Door d = door.GetComponent<Door> ();
			if (d.isOpen) {
				for (int i = 0; i < manager.rooms.Length; i++) {
					if (rooms [i] != currRoom && rooms [i].doors.Contains (door)) {
						adjacentOpenRooms.Add (rooms [i]);
						break;
					}
				}
			}
		}

		if (adjacentOpenRooms.Count >= 1) {
			if (panic)
				Calm ();
			return SetLoiterDestination (adjacentOpenRooms [Random.Range (0, adjacentOpenRooms.Count)].gameObject);
		}
		else {
			if(!ai.isBoarding && !panic)
				Panic ();
			return SetLoiterDestination (currentRoom);
		}
	}

	public Transform SetLoiterDestination(GameObject targetRoom){
		
		Room room = targetRoom.GetComponent<Room> ();
		Transform dest = room.loiterNodes[Random.Range (0, room.loiterNodes.Count)];
		return dest;
	}

	public Transform Decide(GameObject currentRoom, int roamChance){
		if (Random.Range (0, 100) <= roamChance) {
			return SetRoamDestination(currentRoom);
		} else {
			return SetLoiterDestination(currentRoom);
		}
	}

	void Panic(){
		panic = true;
		ai.meshAgent.speed = 20f;
		ai.LOITER_THRESHOLD_MIN = 2;
		ai.LOITER_THRESHOLD_MAX = 3;
		ai.ROAM_PERCENT_MIN = 80;
		ai.ROAM_PERCENT_MAX = 90;
	}

	void Calm(){
		panic = false;
		ai.meshAgent.speed = 9f;
		ai.LOITER_THRESHOLD_MIN = 5;
		ai.LOITER_THRESHOLD_MAX = 8;
		ai.ROAM_PERCENT_MIN = 40;
		ai.ROAM_PERCENT_MAX = 60;
	}
}
