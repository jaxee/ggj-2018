using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadLevelScript : MonoBehaviour {
    
    public void loadLevel()
    {
		ResetScores ();
        SceneManager.LoadScene(1);
    }

	void ResetScores(){
		Manager.score = 0;
		Manager.numberOfHealthySaved = 0;
		Manager.numberOfInfectedSaved = 0;
		Manager.numberOfInfectedEjected = 0;
		Manager.numberOfHealthyEjected = 0;
	}
}
