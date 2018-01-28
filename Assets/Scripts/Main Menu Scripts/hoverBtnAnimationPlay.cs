using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoverBtnAnimationPlay : MonoBehaviour {

    public GameObject buttonVar;
    public GameObject animationVar;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnMouseEnter()
    {
        animationVar.SetActive(true);
        buttonVar.SetActive(false);
    }

    void OnMouseExit()
    {
        animationVar.SetActive(false);
        buttonVar.SetActive(true);
    }
}
