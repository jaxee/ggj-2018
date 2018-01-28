using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonWalk : MonoBehaviour {

	[SerializeField]
	public MeshRenderer[] personAnim;
	public float animSpeed = 0.5f;
	float timer;
	int iter = 0;
	public bool isWalking = true;

	// Use this for initialization
	void Start () {
		timer = animSpeed;

	}

	// Update is called once per frame
	void Update () {
		if (isWalking) {
			if (timer > 0) {
				timer -= Time.deltaTime;
			} else {
				personAnim [iter].enabled = false;
				iter++;
				if (iter >= personAnim.Length) {
					iter = 0;
				}
				personAnim [iter].enabled = true;
				timer = animSpeed;
			}
		}
	}
}
