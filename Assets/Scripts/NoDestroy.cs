using UnityEngine;
using System.Collections;

public class NoDestroy : MonoBehaviour {
	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}
}