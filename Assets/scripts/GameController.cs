using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class GameController : MonoBehaviour {

	public enum PlayerTurn{X_TURN, O_TURN, NUETRAL, O_WINS, X_WINS};
	
	public PlayerTurn turn = PlayerTurn.X_TURN;
	public GUIStyle style;

	public const int BOARD_SIZE = 4;
	public const int NUM_PIECES = 5;

	float doneCoooldown = 1;
	float doneCounter = 0;

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
	const int FONT_SIZE = 24;
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

		board = new Board(BOARD_SIZE, NUM_PIECES);
	}
	
	// Update is called once per frame
	void Update () {
		if(SceneProperties.aiPlaying){
			if(turn == PlayerTurn.X_TURN){
				//TODO update this to not always be x
				Vector2 move = AI.makeMove(board,this);
				BlockController[] blocks = FindObjectsOfType<BlockController>();
				foreach (BlockController block in blocks){
					if(block.x == move.x && block.y == move.y){
						block.simulateClick();
					}
				}
			}
		}

		if(turn == PlayerTurn.O_WINS || turn == PlayerTurn.X_WINS){
			if(Input.GetMouseButton(0) && doneCoooldown <= doneCounter){
				Application.LoadLevel("TitleScreen");
			}
			doneCounter+=Time.deltaTime;
		}
	}

	/**
	 * Returns the board after simulating the move on the given board
	 * */
	public Board simulateMove(Board givenBoard, int x, int y){
		if(givenBoard.positions[x][y] == 0){
			//1 for o, 2 for x
			if(turn == PlayerTurn.O_TURN){
				givenBoard.positions[x][y] = 1;
				givenBoard.oPieces--;
			} else if(turn == PlayerTurn.X_TURN){
				givenBoard.positions[x][y] = 2;
				givenBoard.xPieces--;
			}
		} else{ // It was removed
			givenBoard.positions[x][y] = 0;
			if(turn == PlayerTurn.O_TURN){
				givenBoard.oPieces++;
			} else if(turn == PlayerTurn.X_TURN){
				givenBoard.xPieces++;
			}
		}
		return givenBoard;
	}

	//Swapped x and y to match board representation
	public void move(int y, int x, bool updateTurn){
		if(turn != PlayerTurn.O_WINS && turn != PlayerTurn.X_WINS){
			board = simulateMove(board, x, y);

			int result = checkWin(x,y);
			if(1 == result){
				//O wins
				turn = PlayerTurn.O_WINS;
			} else if(2 == result){
				//x wins
				turn = PlayerTurn.X_WINS;
			}

			Debug.Log("Board");
			for(int i=0;i<BOARD_SIZE;i++){
				Debug.Log(board.positions[i][0]+" "+board.positions[i][1]+" "
				          +board.positions[i][2]+" "+board.positions[i][3]);
			}

			if(updateTurn){
				if(turn == PlayerTurn.O_TURN){
					turn = PlayerTurn.X_TURN;
				} else if(turn == PlayerTurn.X_TURN){
					turn = PlayerTurn.O_TURN;
				}
			}
		}
	}

	/**
	 * Checks if the updated x/y coord leads to a win.
	 * Returns 1 for O winning, 2 for X winning, 0 otherwise;
	 * */
	private int checkWin(int x, int y){
		int piece = board.positions[x][y];
		if( piece == 0){
			return 0;
		}

		//check horizontal
		bool same = true;
		for(int i=0;i<BOARD_SIZE;i++){
			same = (piece==board.positions[i][y]) && same;
		}
		if (same) {
			return board.positions[x][y];
		}

		//check Vertical
		same = true;
		for(int i=0;i<BOARD_SIZE;i++){
			same = (piece==board.positions[x][i]) && same;
		}
		if (same) {
			return piece;
		}

		same = true;
		//check for 4 corners
		same = (piece == board.positions[0][0] && piece == board.positions[BOARD_SIZE-1][0] 
		        && piece == board.positions[0][BOARD_SIZE-1] && piece == board.positions[BOARD_SIZE-1][BOARD_SIZE-1]);
		if (same) {
			return piece;
		}

		if(x-1 >= 0){
			//check up left square
			if(y-1 >= 0){
				if(piece == board.positions[x-1][y] && piece == board.positions[x-1][y-1] 
				   && piece == board.positions[x][y-1]){
					return piece;
				}
			} else if(y+1 < BOARD_SIZE){//check down left square
				if(piece == board.positions[x-1][y] && piece == board.positions[x-1][y+1] 
				   && piece == board.positions[x][y+1]){
					return piece;
				}
			}
		} else if(x+1 < BOARD_SIZE){
			//check up right square
			if(y-1 >= 0){
				if(piece == board.positions[x+1][y] && piece == board.positions[x+1][y-1] 
				   && piece == board.positions[x][y-1]){
					return piece;
				}
			} else if(y+1 < BOARD_SIZE){//check down right square
				if(piece == board.positions[x+1][y] && piece == board.positions[x+1][y+1] 
				   && piece == board.positions[x][y+1]){
					return piece;
				}
			}
		}

		return 0;
	}
	/**
	 * 
	 * Return an ArrayList of Vector2 of x,y coordinates where can move
	 * */
	public ArrayList getMovePositions(int x, int y){
		ArrayList positions = new ArrayList();
		if(x-1 >= 0){
			if(y-1 >= 0){
				if(board.positions[x-1][y-1] == 0){
					positions.Add(new Vector2(x-1, y-1));
				}
			}
			if(y+1 < BOARD_SIZE){//check down left square
				if(board.positions[x-1][y+1] == 0){
					positions.Add(new Vector2(x-1, y+1));
				}
			}
		}
		if(x+1 < BOARD_SIZE){
			//check down right square
			if(y-1 >= 0){
				if(board.positions[x+1][y-1] == 0){
					positions.Add(new Vector2(x+1, y-1));
				}
			}
			if(y+1 < BOARD_SIZE){//check up right square
				if(board.positions[x+1][y+1] == 0){
					positions.Add(new Vector2(x+1, y+1));
				}
			}
		}
		return positions;
	}

	/**
	 * Return an ArrayList of blockControllers of x,y coordinates where can move
	 * swapped x and y
	 * */
	public ArrayList getMoveBlocks(int y, int x){
		ArrayList blockControllers = new ArrayList();
		BlockController[] blocks = GameObject.FindObjectsOfType<BlockController>();
		foreach(Vector2 blockVector in getMovePositions(x,y)){
			foreach(BlockController block in blocks){
				if(block.x == blockVector.y && block.y==blockVector.x){
					blockControllers.Add(block);
				}
			}
		}
		return blockControllers;
	}

	public bool canMove(int y,int x){
		return getMovePositions(x,y).Count != 0;
	}

	void OnGUI(){
		style.fontSize = FONT_SIZE;

		GUI.Label(xPiecesLeftRect, "X Pieces: " + board.xPieces.ToString(), style);
		GUI.Label(oPiecesLeftRect, "O Pieces: " + board.oPieces.ToString(), style);
		//TODO: display player turn
		if(turn == PlayerTurn.O_WINS){
			GUI.Label(winLabelRect, "O Wins!!!", style);
		} else if(turn == PlayerTurn.X_WINS){
			GUI.Label(winLabelRect, "X Wins!!!", style);
		} else if(turn == PlayerTurn.O_TURN){
			GUI.Label(winLabelRect, "O Turn...", style);
		} else if(turn == PlayerTurn.X_TURN){
			GUI.Label(winLabelRect, "X Turn...", style);
		}
		//TODO: add highlighting of winning path
	}
}
