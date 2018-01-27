using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	public static List<Interact> doors = new List<Interact> ();
	public int maxDoorsClosed = 6;

	const int DIFFICULTY_MULTIPLIER = 3;

	public Room[] rooms;
	public int numberOfPeople;
	public int difficulty; // Between 1 and 5

	private int score = 0;

	private int numberOfDoors = 0;
	private int numberOfSickPeople = 0;
	private int numberOfHealthyPeople = 0;

	public GameObject person;

	void Start () {
		numberOfSickPeople = difficulty * DIFFICULTY_MULTIPLIER;
		numberOfHealthyPeople = numberOfPeople - numberOfSickPeople;

		for (int j = 0; j < numberOfPeople; j++) {
			Room room = rooms[Random.Range(0, rooms.Length)];
			Vector3 spawnPos = room.loiterNodes [Random.Range (0, room.loiterNodes.Count)].position;
			GameObject newPerson = Instantiate (person, spawnPos, transform.rotation) as GameObject;

			AIComponent p = newPerson.GetComponent<AIComponent> ();

			if (j < numberOfSickPeople) {
				p.isInfected = true;
			}
		}

		//Debug.Log ("Number of People: " + numberOfPeople + " | sick: " + numberOfSickPeople + " healthy: " + numberOfHealthyPeople);
	}
	
	// Update is called once per frame
	void Update () {
		CheckDoors ();
	}

	//Opens the first door in the list if the list exceeds max count
	public void CheckDoors(){
		if (doors.Count > maxDoorsClosed) {
			doors [0].Trigger ();
		}
	}
}