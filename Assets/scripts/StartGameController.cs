using UnityEngine;
using System.Collections;

public class StartGameController : MonoBehaviour {

	public GameObject twoPlayerButton;
	public GameObject onePlayerButton;
	public GameObject tutorialButton;

	float seconds = 4;
	float counter = 0;
	bool fadeOut = false;

	// Use this for initialization
	void Start () {
		twoPlayerButton.SetActive(false);
		onePlayerButton.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

		if(fadeOut){
			counter += Time.deltaTime;
			if(counter >= seconds){
				counter = seconds;
				fadeOut = false;

			}
			SpriteRenderer renderer = GetComponent<SpriteRenderer>();
			Color c = twoPlayerButton.GetComponent<SpriteRenderer>().material.color;
			c.a = (counter / seconds);
			twoPlayerButton.GetComponent<SpriteRenderer>().material.color = c;
			Color c1 = onePlayerButton.GetComponent<SpriteRenderer>().material.color;
			c1.a = (counter / seconds);
			onePlayerButton.GetComponent<SpriteRenderer>().material.color = c1;
			//fade out tutorial and current button
			Color c3 = onePlayerButton.GetComponent<SpriteRenderer>().material.color;
			c3.a = 1-(counter / seconds);
			onePlayerButton.GetComponent<SpriteRenderer>().material.color = c3;
			Color c2 = renderer.material.color;
			c2.a = 1 - (counter / seconds);
			renderer.material.color = c2;
		}
	}

	void OnMouseDown(){
		Debug.Log("Clicked start");
		twoPlayerButton.SetActive(true);
		onePlayerButton.SetActive(true);
		tutorialButton.SetActive(false);
		this.gameObject.SetActive(false);
		fadeOut = true;
	}
}
