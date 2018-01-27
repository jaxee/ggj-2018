using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

	public int typeOfRoom; // 1 is normal, 2 is hazard, 3 is escape
	public int numberOfDoors;
	private bool[] doorStatus;
	private List<GameObject> Transform = new List<GameObject>();
	
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
	}

	public int getNumberOfDoors () {
		return numberOfDoors;
	}
}
