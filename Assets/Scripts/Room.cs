using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

	public int typeOfRoom; // 1 is normal, 2 is hazard, 3 is escape
	public int numberOfDoors;
	private int numberOfSick;
	private int numberOfHealthy;
	private bool[] doorStatus;

	public Room (string type, int healthy, int sick) {
		typeOfRoom = type;
		numberOfHealthy = healthy;
		numberOfSick = sick;

		createRoom ();
		createPeople ();
	}
	
	void Update () {
		// Keep track of the number of healthy and sick people in a room
	}

	private void createRoom() {

		for (int i = 0; i < numberOfDoors; i++) {
			doorStatus [i] = true;
		}

		if (typeOfRoom == 1) {
			// Create a normal room
		} else if (typeOfRoom == 2) {
			// Create an hazard room
		} else {
			// Create an escape room
		}
	}

	private void createPeople() {
		for (int i = 0; i < numberOfHealthy; i++) {
			// Create healthy people
		}

		for (int j = 0; j < numberOfSick; j++) {
			// Create sick people
		}
	}

	public int getNumberOfDoors () {
		return numberOfDoors;
	}
}
