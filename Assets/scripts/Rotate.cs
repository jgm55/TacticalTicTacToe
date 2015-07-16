using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class Rotate : MonoBehaviour {

	private bool canRotate = false;
    private Board.PlayerTurn turn = Board.PlayerTurn.X_TURN;
    float counter = 0f;
    float TIME_ROTATE = .5f;
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;

	}

    public bool getCanRotate()
    {
        return canRotate;
    }
    public void setCanRotate()
    {
        if(counter > TIME_ROTATE+.1f){
            //Debug.Log ("Change of Turn!");
            iTween.RotateAdd(gameObject, new Vector3(180, 0, 0), TIME_ROTATE);
            if (turn == Board.PlayerTurn.X_TURN)
            {
                turn = Board.PlayerTurn.O_TURN;
            }
            else
            {
                turn = Board.PlayerTurn.X_TURN;
            }
            counter = 0f;
        }
    }
    public Board.PlayerTurn getTurn()
    {
        return turn;
    }
}

