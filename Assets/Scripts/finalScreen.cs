using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class finalScreen : MonoBehaviour {

	public Text healthyEvac;
	public Text healthyEject;
	public Text infectedEvac;
	public Text infectedEject;
	public Text finalScore;

	// Use this for initialization
	void Start () {
		healthyEvac.text = Manager.numberOfHealthySaved.ToString();
		healthyEject.text = Manager.numberOfHealthyEjected.ToString();
		infectedEvac.text = Manager.numberOfInfectedSaved.ToString();
		infectedEject.text = Manager.numberOfInfectedEjected.ToString();

		finalScore.text = Manager.score.ToString();
	}

	void Update() {
		if (Input.anyKeyDown) {
			SceneManager.LoadScene (0);
		}
	}
}
