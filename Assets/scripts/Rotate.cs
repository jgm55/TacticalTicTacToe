using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	public bool canRotate = false;

	Vector3 target = new Vector3(0,0,0);

	
	// Update is called once per frame
	void Update () {

		if (canRotate){
			//Debug.Log ("Change of Turn!");
			canRotate = false;
			iTween.RotateAdd(gameObject,new Vector3(180,0,0),0.5f);
			}

		}
	
	}

