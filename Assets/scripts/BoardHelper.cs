//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using UnityEngine;


namespace AssemblyCSharp
{
	public class BoardHelper
	{
		private static BoardHelper instance;
		private static int BOARD_SIZE;

		static BoardHelper()
		{
			BOARD_SIZE = GameController.BOARD_SIZE;
		}

		public static BoardHelper getInstance(){
			if(instance == null){
				instance = new BoardHelper();
			}
			return instance;
		}

		/**
	 	* Checks if the updated x/y coord leads to a win.
	 	* Returns 1 for O winning, 2 for X winning, 0 otherwise;
	 	* */
		public int checkWin(int x, int y, Board givenBoard){
			int piece = givenBoard.positions[x][y];
			if( piece == 0){
				return 0;
			}
			
			//check horizontal
			bool same = true;
			for(int i=0;i<BOARD_SIZE;i++){
				same = (piece==givenBoard.positions[i][y]) && same;
			}
			if (same) {
				return givenBoard.positions[x][y];
			}
			
			//check Vertical
			same = true;
			for(int i=0;i<BOARD_SIZE;i++){
				same = (piece==givenBoard.positions[x][i]) && same;
			}
			if (same) {
				return piece;
			}
			
			same = true;
			//check for 4 corners
			same = (piece == givenBoard.positions[0][0] && piece == givenBoard.positions[BOARD_SIZE-1][0] 
			        && piece == givenBoard.positions[0][BOARD_SIZE-1] && piece == givenBoard.positions[BOARD_SIZE-1][BOARD_SIZE-1]);
			if (same) {
				return piece;
			}
			
			//check diagonals
			same = true;
			same = (piece == givenBoard.positions[0][0] && piece == givenBoard.positions[3][3] 
			        && piece == givenBoard.positions[1][1] && piece == givenBoard.positions[2][2]);
			if (same) {
				return piece;
			}
			same = true;
			same = (piece == givenBoard.positions[0][3] && piece == givenBoard.positions[1][2] 
			        && piece == givenBoard.positions[2][1] && piece == givenBoard.positions[3][0]);
			if (same) {
				return piece;
			}
			
			if(x-1 >= 0){
				//check up left square
				if(y-1 >= 0){
					if(piece == givenBoard.positions[x-1][y] && piece == givenBoard.positions[x-1][y-1] 
					   && piece == givenBoard.positions[x][y-1]){
						return piece;
					}
				} 
				if(y+1 < BOARD_SIZE){//check down left square
					if(piece == givenBoard.positions[x-1][y] && piece == givenBoard.positions[x-1][y+1] 
					   && piece == givenBoard.positions[x][y+1]){
						return piece;
					}
				}
			} 
			if(x+1 < BOARD_SIZE){
				//check up right square
				if(y-1 >= 0){
					if(piece == givenBoard.positions[x+1][y] && piece == givenBoard.positions[x+1][y-1] 
					   && piece == givenBoard.positions[x][y-1]){
						return piece;
					}
				} 
				if(y+1 < BOARD_SIZE){//check down right square
					if(piece == givenBoard.positions[x+1][y] && piece == givenBoard.positions[x+1][y+1] 
					   && piece == givenBoard.positions[x][y+1]){
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
		public ArrayList getMovePositions(Board givenBoard, int x, int y){
			ArrayList positions = new ArrayList();
			if(x-1 >= 0){
				if(y-1 >= 0){
					if(givenBoard.positions[x-1][y-1] == 0){
						positions.Add(new Vector2(x-1, y-1));
					}
				}
				if(y+1 < BOARD_SIZE){//check down left square
					if(givenBoard.positions[x-1][y+1] == 0){
						positions.Add(new Vector2(x-1, y+1));
					}
				}
			}
			if(x+1 < BOARD_SIZE){
				//check down right square
				if(y-1 >= 0){
					if(givenBoard.positions[x+1][y-1] == 0){
						positions.Add(new Vector2(x+1, y-1));
					}
				}
				if(y+1 < BOARD_SIZE){//check up right square
					if(givenBoard.positions[x+1][y+1] == 0){
						positions.Add(new Vector2(x+1, y+1));
					}
				}
			}
			return positions;
		}

		/**
	 	* Returns the board after simulating the move on the given board
	 	* */
		public Board simulateMove(Board givenBoard, int x, int y){
			if(givenBoard.positions[x][y] == 0){
				//1 for o, 2 for x
				if(givenBoard.turn == Board.PlayerTurn.O_TURN){
					givenBoard.positions[x][y] = 1;
					givenBoard.oPieces--;
				} else if(givenBoard.turn == Board.PlayerTurn.X_TURN){
					givenBoard.positions[x][y] = 2;
					givenBoard.xPieces--;
				}
			} else{ // It was removed
				givenBoard.positions[x][y] = 0;
				if(givenBoard.turn == Board.PlayerTurn.O_TURN){
					givenBoard.oPieces++;
				} else if(givenBoard.turn == Board.PlayerTurn.X_TURN){
					givenBoard.xPieces++;
				}
			}
			return givenBoard;
		}
	}
}
