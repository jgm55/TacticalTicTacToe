using UnityEngine;
using System.Collections;

public class TwoPlayerButtonController : MonoBehaviour {

	public string twoPlayerGameScene = "MainGame";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		Debug.Log("Clicked 2 player start");
		SceneProperties.aiPlaying = false;
		Application.LoadLevel(twoPlayerGameScene);
	}
}
