using UnityEngine;
using System.Collections;

public class TwoPlayerButtonController : MonoBehaviour {
    public string twoPlayerGameScene = "MainGame";
    float counter = 0f;
    bool flipping = false;

    // Update is called once per frame
    void Update()
    {

        if (flipping && counter > .25f)
        {
            Application.LoadLevel(twoPlayerGameScene);
        }
        counter += Time.deltaTime;
    }

    void OnMouseDown()
    {
        //TODO load tutorial page
        this.GetComponentInParent<Rotate>().setCanRotate();
        flipping = true;
        counter = 0f;
		Debug.Log("Clicked 2 player start");
		SceneProperties.aiPlaying = false;
	}
}
