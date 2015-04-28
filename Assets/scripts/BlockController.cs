using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class BlockController : MonoBehaviour {

	public Sprite XPiece, OPiece;
	private Sprite basePiece;
	public GameObject spotlightPrefab;
	GameObject instantiatedSpotlight;

    bool allowedToClick = false;

	//NOTE: Must Set these when instantiate object
	public int x, y;
	public enum BlockState {NUETRAL,O,X};
	public enum BlockHighlightState {SELECTED, MOVE_TO, NUETRAL};
    public enum Action { NONE, MOVED, PICKED_UP, PLACED, SELECTED,DE_SELECTED};

    public BlockState state = BlockState.NUETRAL;

	public BlockHighlightState highlightState = BlockHighlightState.NUETRAL;

	// Use this for initialization
	void Start () {
		basePiece = GetComponent<SpriteRenderer>().sprite;
	}
	
	// Update is called once per frame
	void Update () {
        GameController gc = FindObjectOfType<GameController>();
        state = gc.getState(x,y);
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (state == BlockState.O)
        {
            spriteRenderer.sprite = OPiece;
        }
        else if (state == BlockState.X)
        {
            spriteRenderer.sprite = XPiece;
        }
        else
        {
            spriteRenderer.sprite = basePiece;
        }

		Vector3 position = new Vector3(transform.position.x,transform.position.y, transform.position.z+spotlightPrefab.transform.position.z);

		if(highlightState == BlockHighlightState.MOVE_TO){
			//Highilight it that it can be moved
			if(instantiatedSpotlight == null){
				instantiatedSpotlight = GameObject.Instantiate(spotlightPrefab, position, transform.rotation) as GameObject;
			}
		} else if(highlightState == BlockHighlightState.SELECTED){
			//The block is selected
			if(instantiatedSpotlight == null){
				instantiatedSpotlight = GameObject.Instantiate(spotlightPrefab, position, transform.rotation) as GameObject;
			}
		} else if(highlightState == BlockHighlightState.NUETRAL){
			//remove highlighting
			if(instantiatedSpotlight != null){
				Destroy(instantiatedSpotlight);
			}
		} else {
			Debug.Log("BAD - HIGHLIGHT STATE SET WRONG ON BLOCK");
		}
	}

    public Action highlightSquare(BlockState pieceState)
    {
        GameController gameController = FindObjectOfType<GameController>();
        Action returnAction = Action.NONE;
        if (gameController.board.turn == Board.PlayerTurn.O_TURN && pieceState != BlockState.O)
        {
            return Action.NONE;
        }
        if (gameController.board.turn == Board.PlayerTurn.X_TURN && pieceState != BlockState.X)
        {
            return Action.NONE;
        }
        if (highlightState == BlockHighlightState.SELECTED)
        {
            //De-select a piece
            resetBlocks();
            returnAction = Action.DE_SELECTED;
        }
        else if ((state == BlockState.X && gameController.board.turn == Board.PlayerTurn.X_TURN)
                || (state == BlockState.O && gameController.board.turn == Board.PlayerTurn.O_TURN))
        {
            //Select a piece
            resetBlocks();
            highlightState = BlockHighlightState.SELECTED;
            foreach (BlockController block in gameController.getMoveBlocks(x, y))
            {
                block.highlightState = BlockHighlightState.MOVE_TO;
            }
            returnAction = Action.SELECTED;
        }
        return returnAction;
    }

	public Action clickSquare(BlockState pieceState, bool makeMove){
        GameController gameController = FindObjectOfType<GameController>();
        if (gameController.board.turn == Board.PlayerTurn.O_TURN && pieceState != BlockState.O)
        {
            return Action.NONE;
        } 
        if (gameController.board.turn == Board.PlayerTurn.X_TURN && pieceState != BlockState.X)
        {
            return Action.NONE;
        }
        Action returnAction = Action.NONE;
        if (true)
        {
            if (!SceneProperties.aiPlaying || (SceneProperties.aiPlaying && gameController.board.turn != Board.PlayerTurn.X_TURN || allowedToClick))
            {
                //Check for placing
                if (highlightState == BlockHighlightState.MOVE_TO)
                {
                    if (gameController.board.turn == Board.PlayerTurn.O_TURN)
                    {
                        //state = BlockState.O;
                    }
                    else if (gameController.board.turn == Board.PlayerTurn.X_TURN)
                    {
                        //state = BlockState.X;
                    }
                    BlockController[] blocks = GameObject.FindObjectsOfType<BlockController>();
                    foreach (BlockController block in blocks)
                    {
                        if (block.highlightState == BlockHighlightState.SELECTED)
                        {
                            //found the move from. Remove it.
                            //block.state = BlockState.NUETRAL;
                            //remove it
                            if (makeMove)
                            {
                                gameController.move(block.x, block.y, false);
                            }
                        }
                        //block.highlightState = BlockHighlightState.NUETRAL;
                    }
                    resetBlocks();
                    if (makeMove)
                    {
                        gameController.move(x, y, true);
                    }
                    returnAction = Action.MOVED;
                }
                else if (state == BlockState.NUETRAL)
                {
                    if (gameController.board.turn == Board.PlayerTurn.X_TURN && gameController.board.xPieces > 0)
                    {
                        //state = BlockState.X;
                        if (makeMove)
                        {
                            gameController.move(x, y, true);
                            resetBlocks();
                        }
                    }
                    else if (gameController.board.turn == Board.PlayerTurn.O_TURN && gameController.board.oPieces > 0)
                    {
                        //state = BlockState.O;
                        if (makeMove)
                        {
                            gameController.move(x, y, true);
                            resetBlocks();
                        }
                    }
                    returnAction = Action.PLACED;
                }
                else if (!gameController.canMove(x, y) &&
                        ((state == BlockState.O && gameController.board.turn == Board.PlayerTurn.O_TURN)
               || (state == BlockState.X && gameController.board.turn == Board.PlayerTurn.X_TURN)))
                {
                    // Remove a piece
                    //state = BlockState.NUETRAL;
                    if (makeMove)
                    {
                        gameController.move(x, y, true);
                        resetBlocks();
                    }
                    returnAction = Action.PICKED_UP;
                }
            }
            allowedToClick = false;
        }
        return returnAction;
	}

	private void resetBlocks(){
		BlockController[] blocks = GameObject.FindObjectsOfType<BlockController>();
		foreach(BlockController block in blocks){
			block.highlightState = BlockHighlightState.NUETRAL;
		}
	}

    /**
     * Call by the AI system ONLY, please
     * */
	public void simulateClick(){
        allowedToClick = true;
		clickSquare(BlockState.X, true);
	}
}
