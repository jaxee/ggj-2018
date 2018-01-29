using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sateliteMoveScript : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        transform.RotateAround(Vector3.zero, Vector3.up, 10 * Time.deltaTime);
    }
}
