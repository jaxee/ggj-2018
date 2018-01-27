using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIComponent : MonoBehaviour {
	int LOITER_THRESHOLD_MIN = 10f;
	int LOITER_THRESHOLD_MAX = 20f;

	int ROAM_PERCENT_MIN = 10f;
	int ROAM_PERCENT_MAX = 30f;

	//The max amount of time an AI will stay still
	int loiterThreshold;

	//% chance to Roam when moving
	int roamChance;

	AIBehaviour behaviour;

	GameObject destination;
	public GameObject currentRoom;

	AIBehaviour.State currentState = AIBehaviour.State.Loiter;

	int loiterTime;

	int frameCount = 0;

	// Use this for initialization
	void Start () {
		behaviour = GetComponent<AIBehaviour> ();

		//Different AI will tend to move around more often than others.
		loiterThreshold = Random.Range (LOITER_THRESHOLD_MIN, LOITER_THRESHOLD_MAX);
		loiterTime = loiterThreshold;

		//Some AI are more likely to switch rooms, rather than wander the same room.
		roamChance = Random.Range (ROAM_PERCENT_MIN, ROAM_PERCENT_MAX);
	}

	void Update () {
		
		//Will decide whether or not to move every ~1s.
		if (frameCount >= 60) {
			frameCount = 0;
			//If a random number between 0 and the remaining loiter time is 1, Decide whether to wander in the same room, or move to a different room
			if (Random.Range (0, loiterTime) == 1) {
				currentState = behaviour.Decide (currentRoom, roamChance);
				loiterTime = loiterThreshold;
			} else {
				loiterTime = Mathf.Clamp(--loiterTime, 1, LOITER_THRESHOLD_MAX);
			}
		}
		else
			frameCount++;

		//If we've started roaming but have no destination, set a destination
		if (currentState == AIBehaviour.State.Roam && destination == null) {
			destination = behaviour.SetRoomDestination (currentRoom);
		}
			
	}
}
