using UnityEngine;
using System.Collections;
using AssemblyCSharp;


public class PieceController : MonoBehaviour {

    public GameController.BlockState blockType = GameController.BlockState.X;

    public enum PieceState { HELD,PLACED,RETURNING,START}

    PieceState state = PieceState.START;

    Vector3 startPos;
    Vector3 placePos = Vector3.zero;
    Vector3 returnPlace;
    BlockController fromBlock = null;

    float count = 0f;
    float duration = .5f;
    Vector3 moveDist;

	// Use this for initialization
	void Start () {
        startPos = this.transform.position;
        returnPlace = startPos;
	}
	
	// Update is called once per frame
	void Update () {
        if (state == PieceState.RETURNING)
        {
            this.transform.position += Time.deltaTime* moveDist;
            if (count >= duration)
            {
                if (fromBlock == null)
                {
                    state = PieceState.START;
                }
                else
                {
                    state = PieceState.PLACED;
                }
                count = 0f;
                transform.position = returnPlace;
            }
            count += Time.deltaTime;
        }
        if (state == PieceState.HELD)
        {
            this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 pos = this.transform.position;
            this.transform.position = new Vector3(pos.x, pos.y, startPos.z);
        }
	}

    void OnMouseDown()
    {
        Board.PlayerTurn turn = FindObjectOfType<GameController>().board.turn;
        if ((blockType == GameController.BlockState.X && turn == Board.PlayerTurn.X_TURN) ||
            (blockType == GameController.BlockState.O && turn == Board.PlayerTurn.O_TURN))
        {
            SceneProperties.heldPiece = true;
            state = PieceState.HELD;
            if(state == PieceState.PLACED){
                /*RaycastHit2D[] hits = getRayCastFromScreen();
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.gameObject != this.gameObject)
                    {
                        BlockController block = hit.collider.gameObject.GetComponent<BlockController>();
                        if (block != null)
                        {
                            //block.clickSquare(blockType);
                            block.highlightSquare(blockType);
                        }
                    }
                }*/
            }
            Debug.Log("ON MOUSE DOWN: " + state);
        }
    }

    RaycastHit2D[] getRayCastFromScreen()
    {
        Vector2 camPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return Physics2D.RaycastAll(camPos, Vector2.zero);
    }

    void OnMouseUp()
    {
        Board.PlayerTurn turn = FindObjectOfType<GameController>().board.turn;

        if ((blockType == GameController.BlockState.X && turn == Board.PlayerTurn.X_TURN) ||
            (blockType == GameController.BlockState.O && turn == Board.PlayerTurn.O_TURN))
        {
            //find if block is hit:
            SceneProperties.heldPiece = false;
            state = PieceState.RETURNING;

            //TODO fix this for block controller
            bool onBoard = false;
            RaycastHit2D[] hits = getRayCastFromScreen();
            foreach (RaycastHit2D hit in hits)
            {
                Debug.Log(hit.collider.gameObject);
                if (hit.collider.gameObject != this.gameObject)
                {
                    BlockController block = hit.collider.gameObject.GetComponent<BlockController>();
                    if (block != null)
                    {
                        BlockController.Action actionPerformed = block.clickSquare(fromBlock, false);
                        Debug.Log("actionPerformed: " + actionPerformed);
                        if ((actionPerformed == BlockController.Action.PLACED && returnPlace == startPos)
                            || actionPerformed == BlockController.Action.MOVED && returnPlace == placePos)
                        {
                            block.clickSquare(fromBlock, true);
                            this.transform.position = block.transform.position;
                            state = PieceState.PLACED;
                            placePos = transform.position;
                            returnPlace = placePos;
                            fromBlock = block;
                        }
                        else
                        {
                            //FindObjectOfType<GameController>().undo();
                        }
                    }
                    else if (hit.collider.gameObject.tag == "Board")
                    {
                        onBoard = true;
                    }
                }
            }
            //if not placed and picked up happens, then add back to pile.
            if (state != PieceState.PLACED && onBoard == false)
            {
                if (fromBlock != null && BlockController.Action.PICKED_UP == fromBlock.clickSquare(fromBlock, false))
                {
                    fromBlock.clickSquare(fromBlock, true);
                    state = PieceState.RETURNING;
                    returnPlace = startPos;
                    placePos = Vector3.zero;
                    fromBlock = null;
                }
            }

            Vector3 pos = transform.position;
            moveDist = new Vector3((returnPlace.x - pos.x) / duration, (returnPlace.y - pos.y) / duration, 0);
            Debug.Log("ON MOUSE UP: " + state);
        }
    }
}
