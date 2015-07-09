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
    public enum Action { NONE, MOVED, PICKED_UP, PLACED, SELECTED,DE_SELECTED};
    public GameController.BlockState state;

	// Use this for initialization
	void Start () {
		basePiece = GetComponent<SpriteRenderer>().sprite;
	}
	
	// Update is called once per frame
	void Update () {
        GameController gc = FindObjectOfType<GameController>();
        state = gc.getState(x, y);
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (state == GameController.BlockState.O)
        {
            spriteRenderer.sprite = OPiece;
        }
        else if (state == GameController.BlockState.X)
        {
            spriteRenderer.sprite = XPiece;
        }
        else
        {
            spriteRenderer.sprite = basePiece;
        }

		//this.transform.position = new Vector3(transform.position.x,transform.position.y, transform.position.z+spotlightPrefab.transform.position.z);
	}
    /*
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
    */
    public Action clickSquare(BlockController fromBlock, bool makeMove)
    {
        GameController gameController = FindObjectOfType<GameController>();
        /*if (gameController.board.turn == Board.PlayerTurn.O_TURN && pieceState != BlockState.O)
        {
            return Action.NONE;
        } 
        if (gameController.board.turn == Board.PlayerTurn.X_TURN && pieceState != BlockState.X)
        {
            return Action.NONE;
        }*/
        Action returnAction = Action.NONE;
        if (!SceneProperties.aiPlaying || (SceneProperties.aiPlaying && gameController.board.turn != Board.PlayerTurn.X_TURN || allowedToClick))
        {
            //Check for placing
            if (fromBlock != null && canMoveFromOther(fromBlock))
            {
                if (makeMove)
                {
                    gameController.move(x, y,fromBlock.x,fromBlock.y, true);
                }
                returnAction = Action.MOVED;
            }
            else if (!gameController.canMove(x, y) &&
                   ((state == GameController.BlockState.O && gameController.board.turn == Board.PlayerTurn.O_TURN)
           || (state == GameController.BlockState.X && gameController.board.turn == Board.PlayerTurn.X_TURN)))
            {
                // Remove a piece
                //state = BlockState.NUETRAL;
                if (makeMove)
                {
                    gameController.move(x, y,-1,-1, true);
                }
                returnAction = Action.PICKED_UP;
            }
            else if (state == GameController.BlockState.NUETRAL)
            {
                if (gameController.board.turn == Board.PlayerTurn.X_TURN && gameController.board.xPieces > 0)
                {
                    //state = BlockState.X;
                    if (makeMove)
                    {
                        gameController.move(x, y,-1,-1, true);
                    }
                    returnAction = Action.PLACED;
                }
                else if (gameController.board.turn == Board.PlayerTurn.O_TURN && gameController.board.oPieces > 0)
                {
                    //state = BlockState.O;
                    if (makeMove)
                    {
                        gameController.move(x, y,-1,-1, true);
                    }
                    returnAction = Action.PLACED;
                }
            }
        }
        allowedToClick = false;
        return returnAction;
	}

    private bool canMoveFromOther(BlockController fromBlock)
    {

        if (fromBlock.x - 1 == x && fromBlock.y - 1 == y)
        {
            return true;
        }
        if (fromBlock.x + 1 == x && fromBlock.y - 1 == y)
        {
            return true;
        }
        if (fromBlock.x - 1 == x && fromBlock.y + 1 == y)
        {
            return true;
        }
        if (fromBlock.x + 1 == x && fromBlock.y + 1 == y)
        {
            return true;
        }

        return false;
    }

    /**
     * Call by the AI system ONLY, please
     * */
	public void simulateClick(){
        allowedToClick = true;
		//clickSquare(BlockState.X, true);
	}
}
