using UnityEngine;
using System.Collections;

public class TutorialButtonController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		//TODO load tutorial page
		Debug.Log("Clicked tutorial");
        Application.LoadLevel("Tutorial");
	}
}
