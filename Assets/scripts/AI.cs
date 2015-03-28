using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class AI : MonoBehaviour {

	static public int depth = 2;
	static public int WIN_HUERISTIC = 1000000;
	static public int COMPUTER_NUMBER = 2;
	static public int PLAYER_NUMBER = 1;


	public static Move makeMove(Board board){
		minimax(board,depth,int.MinValue, int.MaxValue, true);
		HashSet<Move> set = getAvailableMoves(board);
		int randNum = Random.Range(0,set.Count);
		int i=0;
		foreach(Move move in set){
			if(i==randNum){
				Debug.Log("Move from AI"+move.ToString());
				return move;
			}
			i++;
		}
		throw new UnityException("AHH BADDDDD");
	}
	/*
	 * Returns ArrayList of Move of all available moves
	 * */
	static HashSet<Move> getAvailableMoves(Board board){

		HashSet<Move> set = new HashSet<Move>();
		for(int i=0; i<board.positions.Length;i++){
			for(int j=0; j<board.positions[i].Length;j++){
				if(board.xPieces > 0 && board.positions[i][j] == 0){
					set.Add(new Move(i,j));
				} else if(board.positions[i][j] == 2) {//TODO make this not always 2
					ArrayList positions = BoardHelper.getInstance().getMovePositions(board,i,j);
					//can remove
					if(positions.Count == 0){
						set.Add(new Move(i,j));
					} else {//add all move positions to set
						foreach(Vector2 vector in positions){
							set.Add(new Move(i,j,vector));
						}
					}
				}
			}
		}
		return set;
	}

	/**
	 * Minimax with alpha beta pruning.
	 * Returns hueristic
	 * */
	static int minimax(Board board, int depth,int alpha, int beta, bool maximize){
		if(depth==0 ){//or terminal node (game over)
			//BoardHelper.getInstance().checkWin(x,y,board)
			return hueristic(board);
		}
		if(maximize){
			int bestValue = int.MinValue;
			foreach(Move move in getAvailableMoves(board)){
				Vector2[] clicks =  move.getClicks();
				BoardHelper helper = BoardHelper.getInstance();
				Board tempBoard = new Board(board);
				if(clicks.Length == 2){
					tempBoard = helper.simulateMove(tempBoard,clicks[0]);
					if(helper.checkWin(clicks[0],tempBoard) == COMPUTER_NUMBER){
						return WIN_HUERISTIC;	
					}
					tempBoard = helper.simulateMove(tempBoard,clicks[1]);
				} else {
					tempBoard = helper.simulateMove(tempBoard,clicks[0]);
				}
				//update turn
				tempBoard.updateTurn();

				int val = minimax(tempBoard,depth-1,alpha,beta,!maximize);
				bestValue = Mathf.Max(bestValue,val);
				alpha = Mathf.Max(alpha,val);
				if(beta <= alpha){
					break;
				}
			}
			return bestValue;
		} else {
			int bestValue = int.MaxValue;
			foreach(Move move in getAvailableMoves(board)){
				Vector2[] clicks =  move.getClicks();
				BoardHelper helper = BoardHelper.getInstance();
				Board tempBoard = new Board(board);
				if(clicks.Length == 2){
					tempBoard = helper.simulateMove(tempBoard,clicks[0]);
					if(helper.checkWin(clicks[0],tempBoard) == PLAYER_NUMBER){
						return -1*WIN_HUERISTIC;	
					}
					tempBoard = helper.simulateMove(tempBoard,clicks[1]);
				} else {
					tempBoard = helper.simulateMove(tempBoard,clicks[0]);
				}
				//update turn
				tempBoard.updateTurn();
				
				int val = minimax(tempBoard,depth-1,alpha,beta,!maximize);
				bestValue = Mathf.Min(bestValue,val);
				beta = Mathf.Min(beta,val);
				if(beta <= alpha){
					break;
				}
			}
			return bestValue;
		}
	}

	static int hueristic(Board board){
		return 0;
	}
	/*
	 *  if depth = 0 or node is a terminal node
        return the heuristic value of node
    if maximizingPlayer
        bestValue := -∞
        for each child of node
            val := minimax(child, depth - 1, FALSE)
            bestValue := max(bestValue, val)
        return bestValue
    else
        bestValue := +∞
        for each child of node
            val := minimax(child, depth - 1, TRUE)
            bestValue := min(bestValue, val)
        return bestValue

(* Initial call for maximizing player *)
minimax(origin, depth, TRUE)
*/
}
