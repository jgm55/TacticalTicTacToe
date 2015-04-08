using UnityEngine;
using System.Collections;

public class PieceController : MonoBehaviour {

    bool held = false;
    Vector3 startPos;

	// Use this for initialization
	void Start () {
        startPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (held)
        {
            this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 pos = this.transform.position;
            this.transform.position = new Vector3(pos.x, pos.y, startPos.z);
        }
	}

    void OnMouseDown()
    {
        SceneProperties.heldPiece = true;
        held = true;
    }

    void OnMouseUp()
    {
        SceneProperties.heldPiece = false;
        held = false;
        //tween to start position
        this.transform.position = startPos;
    }
}
