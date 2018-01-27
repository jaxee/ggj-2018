using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIComponent : MonoBehaviour {
	int LOITER_THRESHOLD_MIN = 5;
	int LOITER_THRESHOLD_MAX = 10;

	int ROAM_PERCENT_MIN = 30;
	int ROAM_PERCENT_MAX = 50;

	float RESIL_MIN = 0.0f;
	float RESIL_MAX = 5.0f;

	float SYMP_MIN = 1200.0f;
	float SYMP_MAX = 7200.0f;

	//The max amount of time an AI will stay still
	int loiterThreshold;

	//% chance to Roam when moving
	int roamChance;

	// How likely they are to get sick
	public float resilience;

	// How long after infected does the player become symptomatic
	private float symptomaticTime = 0.0f;

	public bool isInfected = false;
	public bool isSymptomatic = false;

	private int becomingSymptomatic = 0;

	//Components
	AIBehaviour behaviour;


	Transform destination;
	public GameObject currentRoom;

	AIBehaviour.State currentState = AIBehaviour.State.Loiter;
	NavMeshAgent meshAgent;

	int loiterTime;

	int frameCount = 0;

	// Use this for initialization
	void Start () {
		Debug.Log ("Create person");
		behaviour = GetComponent<AIBehaviour> ();
		meshAgent = GetComponent<NavMeshAgent> ();

		//Different AI will tend to move around more often than others.
		loiterThreshold = Random.Range (LOITER_THRESHOLD_MIN, LOITER_THRESHOLD_MAX);
		loiterTime = loiterThreshold;

		//Some AI are more likely to switch rooms, rather than wander the same room.
		roamChance = Random.Range (ROAM_PERCENT_MIN, ROAM_PERCENT_MAX);

		//Some AI will get sick easier than other AI
		resilience = Random.Range (RESIL_MIN, RESIL_MAX);

		// How long after infected does the player become symptomatic
		symptomaticTime = Random.Range(SYMP_MIN, SYMP_MAX);
	}

	//TODO: don't run while moving
	void Update () {
		//Will decide whether or not to move every ~1s.
		if (frameCount >= 60) {
			frameCount = 0;
			//If a random number between 0 and the remaining loiter time is 1, Decide whether to wander in the same room, or move to a different room
			if (meshAgent.velocity.magnitude < 0.5f) {
				if (Random.Range (0, loiterTime) == 1) {
					destination = behaviour.Decide (currentRoom, roamChance);
					loiterTime = loiterThreshold;
				} else {
					loiterTime--;
					loiterTime = Mathf.Clamp (loiterTime, 2, LOITER_THRESHOLD_MAX);
				}
			}
		} else {
			frameCount++;
		}
		//Consume the destination by putting it into the meshAgent
		if (destination != null) {
			meshAgent.destination = destination.position;
			meshAgent.isStopped = false;
			destination = null;
		}
			
		if (isInfected && !isSymptomatic) {
			Debug.Log ("When they will become symptomatic: " + symptomaticTime);
			if (becomingSymptomatic >= symptomaticTime) {
				isSymptomatic = true;
			}

			becomingSymptomatic++;
		} else if (isInfected && isSymptomatic) {
			// death will happen
			// show some symptoms?
		}
	}
}
