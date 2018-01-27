using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour {
	Camera cam;
	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera> ();
	}

	// Update is called once per frame
	void Update () {

		if (Input.mouseScrollDelta.y > 0f) {
			cam.orthographicSize -= 4;
		} else if (Input.mouseScrollDelta.y < 0f) {
			cam.orthographicSize += 4;
		}
		cam.orthographicSize = Mathf.Clamp (cam.orthographicSize, 10f, 75f);
		/*
		if (Input.mousePosition.x < Screen.width / 6f) {
			transform.position += 2f * -transform.right;
		} else if (Input.mousePosition.x > 5 * Screen.width / 6f) {
			transform.position += 2f * transform.right;
		}

		if (Input.mousePosition.y < Screen.height / 6f) {
			transform.position += 2f * -transform.up;
		} else if (Input.mousePosition.y > 5 * Screen.height / 6f) {
			transform.position += 2f * transform.up;
		}
		*/
		Vector2 screenCenter = new Vector2 (Screen.width / 2, Screen.height / 2);
		Vector2 centerToCursor = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - screenCenter;
		Vector3 addedPos = new Vector3(centerToCursor.x * ((float)Screen.height / (float)Screen.width), centerToCursor.y, 0) / (Screen.height / 6f);

		/*
		if (Mathf.Abs(addedPos.x) > 0.35f)
			addedPos.x -= 0.35f;
		else
			addedPos.x = 0;
		
		if (Mathf.Abs(addedPos.y) > 0.35f)
			addedPos.y -= 0.35f;
		else
			addedPos.y = 0;
		*/

		if(addedPos.magnitude > 0.5f)
			transform.position = Vector3.Lerp(transform.position, transform.position + addedPos, Time.deltaTime * 25f);

		
	}
}
