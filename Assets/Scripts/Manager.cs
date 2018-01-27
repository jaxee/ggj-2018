using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	public Room[] rooms;
	public int numberOfRooms;
	private int numberOfDoors;
	private bool[] allDoors;

	// Use this for initialization
	void Start () {

		rooms = GameObject.FindGameObjectsWithTag("Room");

		for (int i = 0; i < numberOfRooms; i++) {
			numberOfDoors += rooms[i].getNumberOfDoors ();
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
