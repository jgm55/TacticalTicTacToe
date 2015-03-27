using UnityEngine;
using System.Collections;

public class OnePlayerButtonController : MonoBehaviour {

	public string onePlayerGameScene = "MainGame";


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		Debug.Log("Clicked One Player Start");
		SceneProperties.aiPlaying = true;
		Application.LoadLevel(onePlayerGameScene);
	}
}
