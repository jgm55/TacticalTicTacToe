using UnityEngine;
using System.Collections;

public class EnablePopUp : MonoBehaviour {
	public GameObject[] enableThis;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseDown(){
		foreach(GameObject i in enableThis){
			i.SetActive(!i.activeSelf);
		}

	}
}
