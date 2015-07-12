using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class UndoButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        Move move = FindObjectOfType<GameController>().undo();
        //do the opposite of the move
        AnimationHelper.doMove(move);
    }
}
