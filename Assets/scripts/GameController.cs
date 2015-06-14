using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class GameController : MonoBehaviour {
    public enum BlockState { NUETRAL, O, X };

	public GUIStyle style;

	public const int BOARD_SIZE = 4;
	public const int NUM_PIECES = 5;

	float doneCoooldown = 1;
	float doneCounter = 0;
	bool moving = false;

	public Board board;

	//GUI Stuff
	Vector2 resolution;
	float resx;
	float resy;
	Rect xPiecesLeftRect;
	Rect oPiecesLeftRect;
	Rect winLabelRect;

	Vector2 piecesLeftSize = new Vector2(180,30);
	Vector2 winLabelSize = new Vector2(180,30);
	Vector2 xPiecesLeftPos;
	Vector2 oPiecesLeftPos;
	const int FONT_SIZE = 40;

    ArrayList previousBoards;
    int previousBoardsIndex = 0;

	void Awake(){
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	// Use this for initialization
	void Start () {

		xPiecesLeftPos = new Vector2(1280 - piecesLeftSize.x, FONT_SIZE);
		oPiecesLeftPos = new Vector2(0, FONT_SIZE);
		
		resolution = new Vector2(Screen.width, Screen.height);
		resx = resolution.x/1280.0f; // 1280 is the x value of the working resolution
		resy = resolution.y/800.0f; // 800 is the y value of the working resolution
		xPiecesLeftRect = new Rect(xPiecesLeftPos.x*resx,xPiecesLeftPos.y*resy,piecesLeftSize.x*resx,piecesLeftSize.y*resy);
		oPiecesLeftRect = new Rect(oPiecesLeftPos.x*resx,oPiecesLeftPos.y*resy,piecesLeftSize.x*resx,piecesLeftSize.y*resy);
		winLabelRect = new Rect(resolution.x / 2 - winLabelSize.x,40*resy,winLabelSize.x * resx, winLabelSize.y*resy);

		board = new Board(BOARD_SIZE, NUM_PIECES, Board.PlayerTurn.X_TURN);
        previousBoards = new ArrayList();
        previousBoards.Add(board);
	}
	
	// Update is called once per frame
	void Update () {
		if(SceneProperties.aiPlaying){
			if(board.turn == Board.PlayerTurn.X_TURN && !moving){
				//TODO update this to not always be x
				Move move = AI.makeMove(board);
                Debug.Log("Board");
			    /*for(int i=0;i<BOARD_SIZE;i++){
				    Debug.Log(board.positions[i][0]+" "+board.positions[i][1]+" "
				              +board.positions[i][2]+" "+board.positions[i][3]);
			    }//*/
                //Debug.Log(move);
				Vector2[] clicks = move.getClicks();
				clickBlock(clicks[0]);
				if(clicks.Length == 2) {
					StartCoroutine(ClickAfterTime(1f, clicks[1]));
				}
			}
		}

		if(board.turn == Board.PlayerTurn.O_WINS || board.turn == Board.PlayerTurn.X_WINS){
			if(Input.GetMouseButton(0) && doneCoooldown <= doneCounter){
				Application.LoadLevel("TitleScreen");
			}
			doneCounter+=Time.deltaTime;
		}
	}

   
    public void undo()
    {
        board = (Board)previousBoards.GetRange(previousBoardsIndex-1,1)[0];
        previousBoards.RemoveAt(previousBoardsIndex);
        previousBoardsIndex--;
    }

	private void clickBlock(Vector2 click){
		BlockController[] blocks = FindObjectsOfType<BlockController>();
		foreach (BlockController block in blocks){
			if(block.x == click.y && block.y == click.x){
				block.simulateClick();
			}
		}
	}

	IEnumerator ClickAfterTime(float seconds, Vector2 click)
	{
		Debug.Log("starting subroutine");
		moving = true;
		yield return new WaitForSeconds(seconds);
		clickBlock(click);
		moving = false;
		Debug.Log("Done Subroutine");
	}

	//Swapped x and y to match board representation
    public BlockState getState(int y, int x)
    {
        if (board.positions[x][y] == 1)
        {
            return BlockState.O;
        } 
        if (board.positions[x][y] == 2)
        {
            return BlockState.X;
        }
        return BlockState.NUETRAL;
    }

	/*
	 * Call from UI elements to perform a move on the board
	 * */
    public void move(int y, int x, int fromY, int fromX, bool updateTurn)
    {
		if(board.turn != Board.PlayerTurn.O_WINS && board.turn != Board.PlayerTurn.X_WINS){
			board = BoardHelper.getInstance().simulateMove(board, x, y, fromX, fromY);

			int result = BoardHelper.getInstance().checkWin(x,y, board);
			if(1 == result){
				//O wins
				board.turn = Board.PlayerTurn.O_WINS;
			} else if(2 == result){
				//x wins
				board.turn = Board.PlayerTurn.X_WINS;
			}

/*			Debug.Log("Board");
			for(int i=0;i<BOARD_SIZE;i++){
				Debug.Log(board.positions[i][0]+" "+board.positions[i][1]+" "
				          +board.positions[i][2]+" "+board.positions[i][3]);
			}*/

			if(updateTurn){
                board.updateTurn();
			}
            previousBoards.Add(board);
            previousBoardsIndex++;
		}
	}

	/**
	 * Return an ArrayList of blockControllers of x,y coordinates where can move
	 * swapped x and y
	 * */
	public ArrayList getMoveBlocks(int y, int x){
		ArrayList blockControllers = new ArrayList();
		BlockController[] blocks = GameObject.FindObjectsOfType<BlockController>();
		foreach(Vector2 blockVector in BoardHelper.getInstance().getMovePositions(board, x, y)){
			foreach(BlockController block in blocks){
				if(block.x == blockVector.y && block.y==blockVector.x){
					blockControllers.Add(block);
				}
			}
		}
		return blockControllers;
	}

	public bool canMove(int y,int x){
		return BoardHelper.getInstance().getMovePositions(board, x,y).Count != 0;
	}

	void OnGUI(){
		style.fontSize = FONT_SIZE;

		GUI.Label(xPiecesLeftRect, "X Pieces: " + board.xPieces.ToString(), style);
		GUI.Label(oPiecesLeftRect, "O Pieces: " + board.oPieces.ToString(), style);
		//TODO: display player turn
		if(board.turn == Board.PlayerTurn.O_WINS){
			GUI.Label(winLabelRect, "O Wins!!!", style);
		} else if(board.turn == Board.PlayerTurn.X_WINS){
			GUI.Label(winLabelRect, "X Wins!!!", style);
		} else if(board.turn == Board.PlayerTurn.O_TURN){
			GUI.Label(winLabelRect, "O Turn...", style);
		} else if(board.turn == Board.PlayerTurn.X_TURN){
			GUI.Label(winLabelRect, "X Turn...", style);
		}
		//TODO: add highlighting of winning path
	}
}
