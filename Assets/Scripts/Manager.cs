using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

	public static List<Interact> doors = new List<Interact> ();
	public int maxDoorsClosed = 6;

	const int DIFFICULTY_MULTIPLIER = 3;

	int POSITIVE_CONSTANT = 30;
	int NEGATIVE_CONSTANT = -20;

	public Room[] rooms;
	public int numberOfPeople;
	public int difficulty; // Between 1 and 5

	private int score = 0;

	private int numberOfDoors = 0;
	public int numberOfSickPeople;
	public int numberOfHealthyPeople;

	[SerializeField]
	public GameObject[] person;
	private GameObject healthBar;
	private float healthBarWidth;

	public List<string> listOfNames = new List<string> (){
		"Steve",
		"Belinda",
		"Jim",
		"Wendy",
		"Pat",
		"Kat",
		"Darcy",
		"Jackson",
		"Linda",
		"Dervus",
		"David",
		"Muriel",
		"Mortimer",
		"Chauncy",
		"Clark",
		"Kent",
		"Bruce",
		"Wayne",
		"Milles",
		"Peter",
		"Sandra",
		"Debora",
		"Winston",
		"Chris",
		"Jenny",
		"Wilson",
		"Amanda",
		"Ariel",
		"Rotunda",
		"Bill",
		"Bobby",
		"Craig",
		"Crumpit",
		"Apple",
		"Tigress",
		"Roberta",
		"Timantha",
		"RicK",
		"MortY",
		"Timmy",
		"Wilis",
		"Darvey",
		"Liam",
		"Micheal",
		"Barbora",
		"Ellies",
		"Vanesa",
		"Vincent",
		"Bertha",
		"Lisa",
		"Simmons",
		"Pork Chop",
		"Gabriel",
		"Potato Henry",
		"Ol Burby",
		"Regular Ruddy",
		"Wild Urp",
		"Abnormal Ruddy",
		"Patricia",
		"Spaghett"
	};

	public List<string> listOfFacts = new List<string> (){
		"Explores air vents",
		"Eats glue",
		"Paints with hammers",
		"Collects porcelain ducks",
		"Afraid of aluminum cans",
		"Thinks parents are trees",
		"Accidental astronaut ",
		"Dreams of pudding ",
		"Is allergic to cork",
		"Spiritually understands",
		"Can't taste bananas ",
		"Is blind",
		"ERROR CLASSIFIED",
		"Great lover",
		"Was in a frat",
		"Loves hamster parfaits ",
		"Invented chocolate ",
		"Is confused",
		"SO LOST",
		"Time Traveler",
		"Circus Clown",
		"Back seat driver",
		"Channels a dead duck",
		"Bad at poker",
		"Picks nose passionatly",
		"Cries in parking lots",
		"Speed walks",
		"Chases dogs",
		"Climbs others",
		"Eats detergent",
		"Has glasses",
		"Owns a boat",
		"Claims to be a cat",
		"Loves turtles",
		"AHHHHHH",
		"Knows this is a game",
		"Watches cats fart",
		"Is always late",
		"Knows what you did",
		"COFFEEEEEE!",
		"Has a foot cramp",
		"Is deaf",
		"Is scared",
		"Needs space",
		"<IS NOT A ROBOT/>",
		"Can't even",
		"Has the best words",
		"Has an extra toe",
		"Lifts bro",
		"Wears a tupee",
		"Loves ocelots",
		"Has a spice rub",
		"Is moist",
		"Sweats ketchup",
		"Runs in squares",
		"Has seen shit",
		"Wants to see shit",
		"Sees you",
		"Detects farts"
	};

	private string nameOne;

	public Text consoleTxt;
	public Text scoreTxt;
	RectTransform rt;

	void Start () {
		numberOfSickPeople = difficulty * DIFFICULTY_MULTIPLIER;
		numberOfHealthyPeople = numberOfPeople - numberOfSickPeople;

		healthBar = GameObject.FindGameObjectWithTag ("Health");
		rt = healthBar.GetComponent<RectTransform>();

		Debug.Log ("Rect width: " + rt.rect.width);
		healthBarWidth = rt.rect.width;
		rt.sizeDelta = new Vector2(ReMap (numberOfSickPeople, 0, numberOfPeople, 0, healthBarWidth), rt.rect.height);

		Debug.Log ("# sick: " + numberOfSickPeople + " | Rect val: " + ReMap (numberOfSickPeople, 0, numberOfPeople, 0, rt.rect.width));

		for (int j = 0; j < numberOfPeople; j++) {
			Room room = rooms[Random.Range(0, rooms.Length)];
			Vector3 spawnPos = room.loiterNodes [Random.Range (0, room.loiterNodes.Count)].position;
			GameObject newPerson = Instantiate (person[Random.Range(0,person.Length)], spawnPos, transform.rotation) as GameObject;

			AIComponent p = newPerson.GetComponent<AIComponent> ();
			int rn = Random.Range (0, listOfNames.Count-1);
			int rd = Random.Range (0, listOfFacts.Count-1);

			p.personName = listOfNames[rn];
			p.personFact = listOfFacts[rd];

			if (j < numberOfSickPeople) {
				p.isInfected = true;
			}

			consoleTxt.text = "Welcome!\n\nStop the virus from spreading";
		}

		scoreTxt.text = score.ToString();

		//Debug.Log ("Number of People: " + numberOfPeople + " | sick: " + numberOfSickPeople + " healthy: " + numberOfHealthyPeople);
	}
	
	// Update is called once per frame
	void Update () {
		CheckDoors ();
		scoreTxt.text = score.ToString();

		Debug.Log ("Rect width: " + rt.rect.width);
		rt.sizeDelta = new Vector2(ReMap (numberOfSickPeople, 0, numberOfPeople, 0, healthBarWidth), rt.rect.height);

		Debug.Log ("# sick: " + numberOfSickPeople);
	}

	//Opens the first door in the list if the list exceeds max count
	public void CheckDoors(){
		if (doors.Count > maxDoorsClosed) {
			doors [0].Trigger ();
		}
	}

	public void OpenDoor() {
		consoleTxt.text = "Close Door";
	}

	public void CloseDoor() {
		consoleTxt.text = "Open Door";
	}

	public void EscapePodLaunched(int pos, int neg) {

		if (pos == 0 && neg == 0) {
			consoleTxt.text = "Evacuated \n Nobody";
		} else if (pos == 0) {
			consoleTxt.text = "Evacuated \n\n" + "Sick " + neg.ToString();
		} else if (neg == 0) {
			consoleTxt.text = "Evacuated \n\n" + "Normal +" + pos.ToString();
		} else {
			consoleTxt.text = "Evacuated \n\n" + "Normal +" + pos.ToString() + "\nSick " + neg.ToString();
		}
	}

	public void PlayerDied(string name){
		consoleTxt.text = name + " died";
	}

	public void ShowName(string name, string fact) {
		consoleTxt.text = name + "\n\n" + fact;
	}

	public int addPoints () {
		score += POSITIVE_CONSTANT;
		return POSITIVE_CONSTANT;
	}

	public int removePoints() {
		score += NEGATIVE_CONSTANT;
		return NEGATIVE_CONSTANT;
	}

	private float ReMap(float s, float a1, float a2, float b1, float b2)
	{
		return b1 + (s-a1)*(b2-b1)/(a2-a1);
	}
}