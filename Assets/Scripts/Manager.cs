﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	const int DIFFICULTY_MULTIPLIER = 3;

	public Room[] rooms;
	public int numberOfPeople;
	public int difficulty; // Between 1 and 10

	private int score = 0;

	private int numberOfDoors = 0;
	private int numberOfSickPeople = 0;
	private int numberOfHealthyPeople = 0;

	AIComponent[] person;

	void Start () {
		numberOfSickPeople = difficulty * DIFFICULTY_MULTIPLIER;
		numberOfHealthyPeople = numberOfPeople - numberOfSickPeople;

		for (int j = 0; j < numberOfPeople; j++) {
			// Make the correct number of people
		}

		Debug.Log ("Number of Doors: " + numberOfDoors);
		Debug.Log ("Number of People: " + numberOfPeople + " | sick: " + numberOfSickPeople + " healthy: " + numberOfHealthyPeople);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
