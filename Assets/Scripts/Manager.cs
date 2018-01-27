using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	const int DIFFICULTY_MULTIPLIER = 3;

	public Room[] rooms;
	//public GameObject[] rooms;
	public int numberOfPeople;
	public int difficulty; // Between 1 and 10

	private int score = 0;

	private int numberOfDoors = 0;
	private int numberOfSickPeople = 0;
	private int numberOfHealthyPeople = 0;

	// Use this for initialization
	void Start () {
		numberOfSickPeople = difficulty * DIFFICULTY_MULTIPLIER;
		numberOfHealthyPeople = numberOfPeople - numberOfSickPeople;

		for (int i = 0; i < rooms.Length; i++) {
			numberOfDoors += rooms[i].getNumberOfDoors();
			rooms [i].createPeople (numberOfHealthyPeople, numberOfSickPeople);
		}

		Debug.Log ("Number of Doors: " + numberOfDoors);
		Debug.Log ("Number of People: " + numberOfPeople + " | sick: " + numberOfSickPeople + " healthy: " + numberOfHealthyPeople);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
