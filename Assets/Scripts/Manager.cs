using System.Collections;
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

	public GameObject person;

	void Start () {
		numberOfSickPeople = difficulty * DIFFICULTY_MULTIPLIER;
		numberOfHealthyPeople = numberOfPeople - numberOfSickPeople;

		for (int j = 0; j < numberOfPeople; j++) {
			// Can change where the player spawns here
			GameObject newPerson = Instantiate (person, new Vector3(transform.position.x+j, transform.position.y, 0), transform.rotation) as GameObject;

			AIComponent p = newPerson.GetComponent<AIComponent> ();

			if (j < numberOfSickPeople) {
				p.isInfected = true;
			}
		}

		Debug.Log ("Number of People: " + numberOfPeople + " | sick: " + numberOfSickPeople + " healthy: " + numberOfHealthyPeople);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
