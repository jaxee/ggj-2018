using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenuScript : MonoBehaviour {

	public bool paused = false;
	public GameObject pauseMenu;

	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape) && !paused) {
			pauseMenu.SetActive (true);
			timePause ();
		} else if (Input.GetKeyDown (KeyCode.Escape) && paused) {
			pauseMenu.SetActive (false);
			timePlay ();
		}
	}

	public void timePause(){
		Time.timeScale = 0;
		paused = true;
	}

	public void timePlay(){
		Time.timeScale = 1f;
		paused = false;
	}

	public void quitGame(){
		SceneManager.LoadScene (0);
	}
}
