using UnityEngine;
using System.Collections;

public class OnePlayerButtonController : MonoBehaviour {

	public string onePlayerGameScene = "MainGame";
    float counter = 0f;
    bool flipping = false;

    // Update is called once per frame
    void Update()
    {

        if (flipping && counter > .25f)
        {
            Application.LoadLevel(onePlayerGameScene);
        }
        counter += Time.deltaTime;
    }

    void OnMouseDown()
    {
        //TODO load tutorial page
        this.GetComponentInParent<Rotate>().setCanRotate();
        flipping = true;
        counter = 0f;
        Debug.Log("Clicked One Player Start");
        SceneProperties.aiPlaying = true;
    }
}
