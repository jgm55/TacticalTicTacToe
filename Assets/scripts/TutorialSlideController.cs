using UnityEngine;
using System.Collections;

public class TutorialSlideController : MonoBehaviour {

    float difference = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Mathf.Abs(transform.parent.position.x - transform.localPosition.x) < difference)
        {
            this.transform.parent.position = new Vector3(transform.transform.position.x -
                Mathf.Lerp(transform.parent.position.x - transform.localPosition.x, 0, Time.deltaTime), 0, 0);
        }

	}
}
