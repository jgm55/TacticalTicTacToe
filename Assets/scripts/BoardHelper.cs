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
using System.Collections.Generic;
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

        public Move compareBoards(Board board, Board olderBoard)
        {
            List<Vector2> wrongSpots = new List<Vector2>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (board.positions[i][j] != olderBoard.positions[i][j])
                    {
                        Vector2 v = new Vector2(i, j);
                        wrongSpots.Add(v);
                        Debug.Log("Foudn difference at : " + i + " " + j);
                    }
                }
            }


            //Move is...
            //removed if board is 0 and older is x or o and only 1
            //placed if only 1 and alder was 0
            //BUT I want to return the reverse of this, so logic swap now
            if (wrongSpots.Count == 1)
            {
                int x = (int)wrongSpots[0].x;
                int y = (int)wrongSpots[0].y;
                if(board.getStateOfSquare(x,y) == GameController.BlockState.NUETRAL){
                    Move move = new Move(x, y, MoveType.PLACE);
                    return move;
                } else if(olderBoard.getStateOfSquare(x,y) == GameController.BlockState.NUETRAL){
                    Move move = new Move(x, y, MoveType.REMOVE);
                    return move;
                }
                else
                {
                    throw new UnityException("Problem in comparing boards, 2 different non-Nuetral spaces");

                }
            }
            else if (wrongSpots.Count == 2)
            {
                int x = (int)wrongSpots[0].x;
                int y = (int)wrongSpots[0].y;
                //Then there must be a move
                //if new board is nuetral, then it was moved from here
                if (board.getStateOfSquare(x, y) == GameController.BlockState.NUETRAL)
                {
                    Move move = new Move(wrongSpots[1],wrongSpots[0]);
                    return move;
                }//if older board was nuetral, it was moved to here
                else if (olderBoard.getStateOfSquare(x, y) == GameController.BlockState.NUETRAL)
                {
                    Move move = new Move(wrongSpots[0], wrongSpots[1]);
                    return move;
                }
                else
                {
                    throw new UnityException("Problem in comparing boards. 2 spots wrong, but no nuetral.");
                }
            }
            throw new UnityException("Problem in comparing boards, they are not different");
        }

		/**
	 	* Checks if the updated x/y coord leads to a win.
	 	* Returns 1 for O winning, 2 for X winning, 0 otherwise;
	 	* */
		public int checkWin(Vector2 click, Board givenBoard){
			//Debug.Log("checkWin: "+click);
			return checkWin((int)click.x,(int)click.y,givenBoard);
		}
		/**
	 	* Checks if the updated x/y coord leads to a win.
	 	* Returns 1 for O winning, 2 for X winning, 0 otherwise;
	 	* */
        public int checkWin(int x, int y, Board givenBoard)
        {
            Vector2 winLoc1 = getWin(x, y, givenBoard)[0];
            return givenBoard.positions[(int)winLoc1.x][(int)winLoc1.y];
        }
        public List<Vector2> getWin(int x, int y, Board givenBoard)
        {
            int piece = givenBoard.positions[x][y];
			if( piece == 0){
                return new List<Vector2> {new Vector2(0,0)};
			}
            List<Vector2> toReturn = new List<Vector2>(4);
			//check horizontal
			bool same = true;
			for(int i=0;i<BOARD_SIZE;i++){
				same = (piece==givenBoard.positions[i][y]) && same;
                toReturn.Add(new Vector2(i,y));
			}
			if (same) {
				return toReturn;
			}
			
			//check Vertical
            toReturn = new List<Vector2>(4);
			same = true;
			for(int i=0;i<BOARD_SIZE;i++){
				same = (piece==givenBoard.positions[x][i]) && same;
                toReturn.Add(new Vector2(x, i));
			}
			if (same) {
                return toReturn;
			}
			
			same = true;
			//check for 4 corners
			same = (piece == givenBoard.positions[0][0] && piece == givenBoard.positions[BOARD_SIZE-1][0] 
			        && piece == givenBoard.positions[0][BOARD_SIZE-1] && piece == givenBoard.positions[BOARD_SIZE-1][BOARD_SIZE-1]);
			if (same) {
                return new List<Vector2> { new Vector2(0,0), new Vector2(BOARD_SIZE - 1,0), new Vector2(0, BOARD_SIZE - 1), new Vector2(BOARD_SIZE - 1, BOARD_SIZE - 1) };
			}
			
			//check diagonals
			same = true;
			same = (piece == givenBoard.positions[0][0] && piece == givenBoard.positions[3][3] 
			        && piece == givenBoard.positions[1][1] && piece == givenBoard.positions[2][2]);
			if (same) {
                return new List<Vector2> { new Vector2(0, 0), new Vector2(1, 1), new Vector2(2, 2), new Vector2(3, 3) };

			}
			same = true;
			same = (piece == givenBoard.positions[0][3] && piece == givenBoard.positions[1][2] 
			        && piece == givenBoard.positions[2][1] && piece == givenBoard.positions[3][0]);
			if (same) {
                return new List<Vector2> { new Vector2(0, 3), new Vector2(1, 2), new Vector2(2, 1), new Vector2(3, 0) };
			}
			
			if(x-1 >= 0){
				//check up left square
				if(y-1 >= 0){
					if(piece == givenBoard.positions[x-1][y] && piece == givenBoard.positions[x-1][y-1] 
					   && piece == givenBoard.positions[x][y-1]){
                           return new List<Vector2> { new Vector2(x-1, y), new Vector2(x-1, y-1), new Vector2(x, y-1), new Vector2(x, y) };
					}
				} 
				if(y+1 < BOARD_SIZE){//check down left square
					if(piece == givenBoard.positions[x-1][y] && piece == givenBoard.positions[x-1][y+1] 
					   && piece == givenBoard.positions[x][y+1]){
                           return new List<Vector2> { new Vector2(x - 1, y), new Vector2(x - 1, y + 1), new Vector2(x, y + 1), new Vector2(x, y) };
					}
				}
			} 
			if(x+1 < BOARD_SIZE){
				//check up right square
				if(y-1 >= 0){
					if(piece == givenBoard.positions[x+1][y] && piece == givenBoard.positions[x+1][y-1] 
					   && piece == givenBoard.positions[x][y-1]){
                           return new List<Vector2> { new Vector2(x + 1, y), new Vector2(x + 1, y - 1), new Vector2(x, y - 1), new Vector2(x, y) }; ;
					}
				} 
				if(y+1 < BOARD_SIZE){//check down right square
					if(piece == givenBoard.positions[x+1][y] && piece == givenBoard.positions[x+1][y+1] 
					   && piece == givenBoard.positions[x][y+1]){
                           return new List<Vector2> { new Vector2(x + 1, y), new Vector2(x + 1, y + 1), new Vector2(x, y + 1), new Vector2(x, y) }; ;
					}
				}
			}

            return new List<Vector2> { new Vector2(0, 0) };
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
		public Board simulateMove(Board givenBoard, Vector2 position){
            return simulateMove(givenBoard, (int)position.x, (int)position.y, -1, -1);
		}
		/**
	 	* Returns the board after simulating the move on the given board
         * pass in -1 for fromX and fromY if it is not a moved piece
	 	* */
		public Board simulateMove(Board givenBoard, int x, int y, int fromX, int fromY){
			if(givenBoard.positions[x][y] == 0){
				//1 for o, 2 for x
				if(givenBoard.turn == Board.PlayerTurn.O_TURN){
					givenBoard.positions[x][y] = 1;
                    if (fromX != -1 && fromY != -1)
                    {
                        givenBoard.positions[fromX][fromY] = 0;
                    }
                    else
                    {
                        givenBoard.oPieces--;
                    }
				} else if(givenBoard.turn == Board.PlayerTurn.X_TURN){
					givenBoard.positions[x][y] = 2;
                    if (fromX != -1 && fromY != -1)
                    {
                        givenBoard.positions[fromX][fromY] = 0;
                    }
                    else
                    {
                        givenBoard.xPieces--;
                    }
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

