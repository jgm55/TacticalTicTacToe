using UnityEngine;
using System.Collections;

public class TutorialButtonController : MonoBehaviour {

    float counter = 0f;
    bool flipping = false;
	
	// Update is called once per frame
	void Update () {

        if(flipping && counter > .25f){
            Application.LoadLevel("Tutorial");
        }
        counter += Time.deltaTime;
	}

	void OnMouseDown(){
		//TODO load tutorial page
		Debug.Log("Clicked tutorial");
        this.GetComponentInParent<Rotate>().setCanRotate();
        flipping = true;
        counter = 0f;
	}
}
