using UnityEngine;
using System.Collections;

public class Play_Button : MonoBehaviour {
	public GameObject tutorialButton;
	void OnMouseDown(){
		this.GetComponentInParent<Rotate>().canRotate = true;
		tutorialButton.GetComponent<Rotate>().canRotate = true;
	}
}
