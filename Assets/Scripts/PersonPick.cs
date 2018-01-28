using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonPick : MonoBehaviour {

	[SerializeField]
	public GameObject[] people;

	// Use this for initialization
	void Start () {
		int choice = Random.Range (0, people.Length - 1);
		Instantiate (people [choice], transform.position + new Vector3(0,-2.04f,0),transform.rotation,transform);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
