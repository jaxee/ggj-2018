using UnityEngine;
using System.Collections;

public class NoDestroy : MonoBehaviour {
	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}


	void Start(){
			if (GameObject.FindGameObjectsWithTag ("Music").Length > 1) {
			Destroy (gameObject);
		}
	}

}