using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class GameController : MonoBehaviour {

	//Jon's illconceived code:
	public GameObject turnIndicator;
    public AudioSource piecePlaceSound;
    public enum BlockState { NUETRAL, O, X };

    private const float UNDO_DELAY = 1f;
    private float undoCounter = 0f;

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
		if(SceneProperties.aiPlaying && undoCounter > UNDO_DELAY){
            if (board.turn == SceneProperties.aiTurn && !moving)
            {
                Move m = AI.makeMove(board);
                Debug.Log("Move: " + m);
                AnimationHelper.doMove(m);
                this.move(m, true, true);
			}
		}
        undoCounter += Time.deltaTime;
		if(board.turn == Board.PlayerTurn.O_WINS || board.turn == Board.PlayerTurn.X_WINS){
			if(Input.GetMouseButton(0) && doneCoooldown <= doneCounter){
				Application.LoadLevel("TitleScreen");
			}
			doneCounter+=Time.deltaTime;
		}
	}

    public BlockState turnToBlockType()
    {
        switch (board.turn)
        {
            case Board.PlayerTurn.X_TURN:
                return BlockState.X;
            case Board.PlayerTurn.O_TURN:
                return BlockState.O;
            default:
                throw new UnityException("Cannot change type "+board.turn+ " into a blockstate");
        }
    }

    public Move undo()
    {
        if (previousBoardsIndex == 0)
        {
            throw new UnityException("Cannot undo more");
        }
        Board newerBoard = (Board)previousBoards[previousBoardsIndex];
        previousBoards.RemoveAt(previousBoardsIndex);
        previousBoardsIndex--;

        Board oldBoard = (Board)previousBoards[previousBoardsIndex];
        Move m = BoardHelper.getInstance().compareBoards(newerBoard, oldBoard);
        //move(m, false);
        flipSign();
        board = oldBoard;
        Debug.Log("Board after undo: \n" + board);
        undoCounter = 0f;
        return m;
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

    public void move(Move m, bool updateTurn, bool updateStack)
    {
        if (m.getPositionTwo().x != -1)
        {
            move((int)m.getPositionTwo().y, (int)m.getPositionTwo().x, (int)m.getPositionOne().y, (int)m.getPositionOne().x, updateTurn, updateStack);
        }
        else
        {
            move((int)m.getPositionOne().y, (int)m.getPositionOne().x, -1, -1, updateTurn, updateStack);
        }
    }

	/*
	 * Call from UI elements to perform a move on the board
	 * */
    public void move(int y, int x, int fromY, int fromX, bool updateTurn, bool updateStack=true)
    {
        Board newBoard = new Board(board);
        if (newBoard.turn != Board.PlayerTurn.O_WINS && newBoard.turn != Board.PlayerTurn.X_WINS)
        {
            newBoard = BoardHelper.getInstance().simulateMove(newBoard, x, y, fromX, fromY);

			int result = BoardHelper.getInstance().checkWin(x,y, newBoard);
			if(1 == result){
				//O wins
                newBoard.turn = Board.PlayerTurn.O_WINS;
			} else if(2 == result){
				//x wins
                newBoard.turn = Board.PlayerTurn.X_WINS;
			}

			Debug.Log("Board\n" + newBoard);
			

			if(updateTurn){
                newBoard.updateTurn();

                flipSign();
			}
            if (updateStack)
            {
                previousBoards.Add(newBoard);
                previousBoardsIndex++;
            }
            board = newBoard;
		}
	}

    private void flipSign()
    {
        //added to turn indicator
        Rotate rotateTurn = turnIndicator.GetComponent<Rotate>();
        rotateTurn.canRotate = true;
        piecePlaceSound.Play();
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
		/*style.fontSize = FONT_SIZE;
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
		//TODO: add highlighting of winning path*/
	}
}
