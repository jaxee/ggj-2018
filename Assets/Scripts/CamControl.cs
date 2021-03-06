using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour {
	Camera cam;
	float smoothing = 20f;
	public bool scrollLock = false;
	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera> ();
	}

	// Update is called once per frame
	void Update () {

		if (Input.mouseScrollDelta.y > 0f) {
			cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, cam.orthographicSize - 8, Time.deltaTime * smoothing);
		} else if (Input.mouseScrollDelta.y < 0f) {
			cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, cam.orthographicSize + 8, Time.deltaTime * smoothing);
		}

		if (Input.GetMouseButtonDown (1))
			scrollLock = !scrollLock;
		

		if (!scrollLock) {
			
			cam.orthographicSize = Mathf.Clamp (cam.orthographicSize, 20f, 75f);
			Vector2 screenCenter = new Vector2 (Screen.width / 2, Screen.height / 2);
			Vector2 centerToCursor = new Vector2 (Input.mousePosition.x, Input.mousePosition.y) - screenCenter;
			Vector3 addedPos = new Vector3 (centerToCursor.x * ((float)Screen.height / (float)Screen.width), centerToCursor.y, 0) / (Screen.height / 6f);

			if (addedPos.magnitude > 1.5f)
				transform.position = Vector3.Lerp (transform.position, transform.position + addedPos, Time.deltaTime * smoothing);

			transform.position = new Vector3 (Mathf.Clamp (transform.position.x, -200f, 0), Mathf.Clamp (transform.position.y, 150, 300f), transform.position.z);
		}
		
	}
}
