using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class AIComponent : MonoBehaviour {
	int LOITER_THRESHOLD_MIN = 5;
	int LOITER_THRESHOLD_MAX = 8;

	int ROAM_PERCENT_MIN = 40;
	int ROAM_PERCENT_MAX = 60;

	int RESIL_MIN = 70;
	int RESIL_MAX = 100;

	float SYMP_MIN = 10.0f;
	float SYMP_MAX = 20.0f;

	float DEATH_MIN = 90f;
	float DEATH_MAX = 120f;
	float deathTimeLength;
	float deathTimer;
	public float currentDeathCount;

	//The max amount of time an AI will stay still
	int loiterThreshold;

	//% chance to Roam when moving
	int roamChance;

	// How likely they are to get sick
	public int resilience;

	// How long after infected does the player become symptomatic
	private float symptomaticTime = 0.0f;
	private float becomingSymptomatic = 0;

	public bool isInfected = false;
	public bool isSymptomatic = false;

	bool sneezing = false;
	public bool isBoarding = false;

	public string personName = null;
	public string personFact = null;

	//Components
	AIBehaviour behaviour;
	public NavMeshAgent meshAgent;
	MeshRenderer meshRenderer;
	Canvas canvas;
	Manager manager;

	public Material infectedMat;
	public GameObject sick_Particles;
	public GameObject death_Particles;

	Transform destination;
	public GameObject currentRoom;

	//AIBehaviour.State currentState = AIBehaviour.State.Loiter;

	int loiterTime;
	int frameCount = 0;


	// Use this for initialization
	void Start () {
		behaviour = GetComponent<AIBehaviour> ();
		meshAgent = GetComponent<NavMeshAgent> ();
		meshRenderer = GetComponent<MeshRenderer> ();
		canvas = GetComponent<Canvas> ();
		manager = GameObject.FindObjectOfType<Manager> ();

		//Different AI will tend to move around more often than others.
		loiterThreshold = Random.Range (LOITER_THRESHOLD_MIN, LOITER_THRESHOLD_MAX);
		loiterTime = loiterThreshold;

		//Some AI are more likely to switch rooms, rather than wander the same room.
		roamChance = Random.Range (ROAM_PERCENT_MIN, ROAM_PERCENT_MAX);

		//Some AI will get sick easier than other AI
		resilience = Random.Range (RESIL_MIN, RESIL_MAX);

		// How long after infected does the player become symptomatic
		symptomaticTime = Random.Range(SYMP_MIN, SYMP_MAX);

		//How long after infected does the player have to live
		deathTimeLength = Random.Range(DEATH_MIN, DEATH_MAX);
		deathTimer = Time.time;
	}

	//TODO: don't run while moving
	void Update () {
		if (!isBoarding) {
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

			//Stop the agent if they reach their destination
			if (meshAgent.remainingDistance <= meshAgent.stoppingDistance) {
				meshAgent.isStopped = true;
			}
			
			if (isInfected) {
				//Makes sure the material changes
				//Not necessary, just for debug
				if (meshRenderer.material != infectedMat) {
					meshRenderer.material = infectedMat;
				}
				currentDeathCount = Time.time - deathTimer;
				//If not symptomatic and not 'sneezing' do so for the first time
				if (!isSymptomatic && !sneezing) {
					StartCoroutine (Sneezing (true));
				} else {
					if (!sneezing)
						StartCoroutine (Sneezing ());
					// death will happen
				}

				if (currentDeathCount > deathTimeLength) {
					Instantiate (death_Particles, transform.position, Quaternion.identity);
					destoryPlayer ();
				}
			}
		}

	}

	public void Infect(){
		meshRenderer.material = infectedMat;
		becomingSymptomatic = Time.time;
		deathTimer = Time.time;
	}

	IEnumerator Sneezing(bool first = false){
		sneezing = true;
		float wait = Random.Range (SYMP_MIN, SYMP_MAX);
		//First time displaying symptoms takes an extra 10s
		if (first)
			wait += 10f;
		yield return new WaitForSeconds (wait);
		Instantiate (sick_Particles, transform.position, Quaternion.identity, transform);
		if (first)
			isSymptomatic = true;
		sneezing = false;
	}

	public void destoryPlayer () {
		currentRoom.GetComponent<Room> ().playersInRoom.Remove (gameObject);
		Destroy (gameObject);
	}

	void OnMouseOver()
	{
		manager.ShowName (personName, personFact);
	}
}

