using UnityEngine;
using System.Collections;

public class OnClickGoScene : MonoBehaviour {

    public string scene = "";

    public float enableDelay = 0f;
    float enableCounter = 0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        enableCounter += Time.deltaTime;
	}

    void OnMouseDown()
    {

        if (enableCounter > enableDelay)
        {
            Application.LoadLevel(scene);
        }
    }
}
