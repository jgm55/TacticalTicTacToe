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
    //Jon
    //Cooldown in seconds until can click. Make this the animation speed
	float doneCoooldown = 1;
	float doneCounter = 0;
	bool moving = false;
	public GameObject x_winscreen;
	public GameObject o_winscreen;

	public Board board;

	const int FONT_SIZE = 40;

    ArrayList previousBoards;
    int previousBoardsIndex = 0;

	void Awake(){
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	// Use this for initialization
	void Start () {

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
                AnimationHelper.doMove(m);
                this.move(m, true, true);                
			}
		}
        undoCounter += Time.deltaTime;
        //Jon
        //Look here is the win stuff.
		if(board.turn == Board.PlayerTurn.O_WINS || board.turn == Board.PlayerTurn.X_WINS){
			if(Input.GetMouseButton(0) && doneCoooldown <= doneCounter){
				Application.LoadLevel("TitleScreen");
			}
            //Add your jawn here, bro.
            //Then update doneCooldown to be the animation speed, probably
			if(board.turn == Board.PlayerTurn.X_WINS){
				x_winscreen.SetActive(true);
			}
			else if (board.turn == Board.PlayerTurn.O_WINS){
				o_winscreen.SetActive(true);
			}
			doneCounter+=Time.deltaTime;
		}
        if (board.turn != Board.PlayerTurn.O_WINS && board.turn != Board.PlayerTurn.X_WINS)
        {
            if (turnIndicator.GetComponent<Rotate>().getTurn() != this.board.turn)
            {
                flipSign();
            }
        }
        else
        {
            if (turnIndicator.GetComponent<Rotate>().getTurn() != Board.PlayerTurn.X_TURN && board.turn == Board.PlayerTurn.X_WINS)
            {
                flipSign();
            }
            else if (turnIndicator.GetComponent<Rotate>().getTurn() != Board.PlayerTurn.O_TURN && board.turn == Board.PlayerTurn.O_WINS)
            {
                flipSign();
            }
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
        board = oldBoard;
        Debug.Log("Board after undo: \n" + board);
        undoCounter = 0f;
        if (!piecePlaceSound.isPlaying)
            piecePlaceSound.Play();

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
        if(!piecePlaceSound.isPlaying)
            piecePlaceSound.Play();
        Board newBoard = new Board(board);
        if (newBoard.turn != Board.PlayerTurn.O_WINS && newBoard.turn != Board.PlayerTurn.X_WINS)
        {
            newBoard = BoardHelper.getInstance().simulateMove(newBoard, x, y, fromX, fromY);

			int result = BoardHelper.getInstance().checkWin(x,y, newBoard);
			if(1 == result){
				//O wins
                newBoard.turn = Board.PlayerTurn.O_WINS;
                AnimationHelper.highlightPlaces(BoardHelper.getInstance().getWin(x, y, newBoard));
			} else if(2 == result){
				//x wins
                newBoard.turn = Board.PlayerTurn.X_WINS;
			}

			Debug.Log("Board\n" + newBoard);
			

			if(updateTurn){
                newBoard.updateTurn();

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
        rotateTurn.setCanRotate();
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
}
