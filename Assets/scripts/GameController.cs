using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public enum PlayerTurn{X_TURN, O_TURN, NUETRAL, O_WINS, X_WINS};
	
	public PlayerTurn turn = PlayerTurn.X_TURN;
	public GUIStyle style;

	public int oPieces = 5;
	public int xPieces = 5;

	public const int BOARD_SIZE = 4;

	int [][] board;

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

		board = new int[BOARD_SIZE][];
		for(int i=0; i<BOARD_SIZE; i++){
			int[] temp = new int[BOARD_SIZE];
			for(int j=0; j<BOARD_SIZE; j++){
				temp[j] = 0;
				//board[i][j] = 0;
			}
			board[i] = temp;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(SceneProperties.aiPlaying){
			if(turn == PlayerTurn.X_TURN){
				//TODO update this to not always be x
				Vector2 move = AI.makeMove(board,xPieces,this);
				BlockController[] blocks = FindObjectsOfType<BlockController>();
				foreach (BlockController block in blocks){
					if(block.x == move.x && block.y == move.y){
						block.simulateClick();
					}
				}
			}
		}
	}

	//Swapped x and y to match board representation
	public void move(int y, int x, bool updateTurn){
		if(turn != PlayerTurn.O_WINS && turn != PlayerTurn.X_WINS){
			if(board[x][y] == 0){
				//1 for o, 2 for x
				if(turn == PlayerTurn.O_TURN){
					board[x][y] = 1;
					oPieces--;
					if(updateTurn){
						turn = PlayerTurn.X_TURN;
					}
				} else if(turn == PlayerTurn.X_TURN){
					board[x][y] = 2;
					xPieces--;
					if(updateTurn){
						turn = PlayerTurn.O_TURN;
					}
				}
			} else{ // It was removed
				board[x][y] = 0;
				if(turn == PlayerTurn.O_TURN){
					oPieces++;
					if(updateTurn){
						turn = PlayerTurn.X_TURN;
					}
				} else if(turn == PlayerTurn.X_TURN){
					xPieces++;
					if(updateTurn){
						turn = PlayerTurn.O_TURN;
					}
				}
			}

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
				Debug.Log(board[i][0]+" "+board[i][1]+" "+board[i][2]+" "+board[i][3]);
			}
		}
	}

	/**
	 * Checks if the updated x/y coord leads to a win.
	 * Returns 1 for O winning, 2 for X winning, 0 otherwise;
	 * */
	private int checkWin(int x, int y){
		int piece = board[x][y];
		if( piece == 0){
			return 0;
		}

		//check horizontal
		bool same = true;
		for(int i=0;i<BOARD_SIZE;i++){
			same = (piece==board[i][y]) && same;
		}
		if (same) {
			return board[x][y];
		}

		//check Vertical
		same = true;
		for(int i=0;i<BOARD_SIZE;i++){
			same = (piece==board[x][i]) && same;
		}
		if (same) {
			return piece;
		}

		same = true;
		//check for 4 corners
		same = (piece == board[0][0] && piece == board[BOARD_SIZE-1][0] 
		        && piece == board[0][BOARD_SIZE-1] && piece == board[BOARD_SIZE-1][BOARD_SIZE-1]);
		if (same) {
			return piece;
		}

		if(x-1 >= 0){
			//check up left square
			if(y-1 >= 0){
				if(piece == board[x-1][y] && piece == board[x-1][y-1] 
				        && piece == board[x][y-1]){
					return piece;
				}
			} else if(y+1 < BOARD_SIZE){//check down left square
				if(piece == board[x-1][y] && piece == board[x-1][y+1] 
				   && piece == board[x][y+1]){
					return piece;
				}
			}
		} else if(x+1 < BOARD_SIZE){
			//check up right square
			if(y-1 >= 0){
				if(piece == board[x+1][y] && piece == board[x+1][y-1] 
				   && piece == board[x][y-1]){
					return piece;
				}
			} else if(y+1 < BOARD_SIZE){//check down right square
				if(piece == board[x+1][y] && piece == board[x+1][y+1] 
				   && piece == board[x][y+1]){
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
				if(board[x-1][y-1] == 0){
					positions.Add(new Vector2(x-1, y-1));
				}
			}
			if(y+1 < BOARD_SIZE){//check down left square
				if(board[x-1][y+1] == 0){
					positions.Add(new Vector2(x-1, y+1));
				}
			}
		}
		if(x+1 < BOARD_SIZE){
			//check down right square
			if(y-1 >= 0){
				if(board[x+1][y-1] == 0){
					positions.Add(new Vector2(x+1, y-1));
				}
			}
			if(y+1 < BOARD_SIZE){//check up right square
				if(board[x+1][y+1] == 0){
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
		//TODO: display moves left
		style.fontSize = FONT_SIZE;

		GUI.Label(xPiecesLeftRect, "X Pieces: " + xPieces.ToString(), style);
		GUI.Label(oPiecesLeftRect, "O Pieces: " + oPieces.ToString(), style);
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
		//TODO: display winner and lock game out
		//TODO: add highlighting of winning path
	}

	void OnMouseDown(){
		if(turn == PlayerTurn.O_WINS || turn == PlayerTurn.X_WINS){
			Application.LoadLevel("TitleScreen");
		}
	}

	/*public void remove(){
		if(turn == PlayerTurn.X_TURN){
			xPieces++;
			turn = GameController.PlayerTurn.O_TURN;
		} else if( turn == PlayerTurn.O_TURN){
			oPieces++;
			turn = GameController.PlayerTurn.X_TURN;
		}
	}*/
}
