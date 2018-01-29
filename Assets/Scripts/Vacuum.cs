using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vacuum : MonoBehaviour {
	public float force = 900000f;
	bool pull = true;
	Room room;

	Manager m;
	int negScoreDiff;
	int posScoreDiff;

	// Use this for initialization
	void Start () {
		room =transform.root.GetComponent<Room> ();
	}
	
	// Update is called once per frame
	void Update () {
		foreach (GameObject go in room.playersInRoom) {
			if (go.GetComponent<AIComponent>().pull) {
				go.transform.GetComponent<Rigidbody> ().AddForce ((transform.position - go.transform.position) * 10f, ForceMode.Force);
			}
		}
			
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.tag == "Player") {
			//DO SCORE HERE
			c.GetComponent<AIComponent>().pull = false;
			c.transform.GetComponent<Rigidbody> ().AddForce (Vector3.down * force, ForceMode.Acceleration);
		}
	}

	void OnTriggerStay(Collider c)
	{
		if (c.tag == "Player") {
			c.transform.GetComponent<Rigidbody> ().AddForce (Vector3.down * force, ForceMode.Acceleration);
		}
	}

	void OnTriggerExit(Collider c)
	{
		if (c.tag == "Player") {
			AIComponent p = c.GetComponent<AIComponent> ();
			m = GameObject.FindObjectOfType<Manager> ();

			if (p.isInfected) {
				posScoreDiff += m.addPoints ();
				m.numberOfSickPeople--;
			} else {
				negScoreDiff += m.removePoints ();
				m.numberOfHealthyPeople--;
			}

			m.numberOfPeople--;

			StartCoroutine(DelayDestroy (c.GetComponent<AIComponent> ()));
		}
	}

	IEnumerator DelayDestroy(AIComponent g)
	{
		m.AirLockFinished (posScoreDiff, negScoreDiff);
		yield return new WaitForSeconds (5f);
		g.destoryPlayer ();
	}
}
