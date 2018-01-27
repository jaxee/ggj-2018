using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	public Room[] rooms;
	public int numberOfPeople;
	public int difficulty;
	private int numberOfDoors = 0;

	// Use this for initialization
	void Start () {

		for (int i = 0; i < rooms.Length; i++) {
			numberOfDoors += rooms[i].getNumberOfDoors();
		}

		Debug.Log ("Number of Doors: " + numberOfDoors);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
