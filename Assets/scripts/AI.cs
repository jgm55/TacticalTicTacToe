using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class AI : MonoBehaviour {

	public int depth = 2;

	public static Move makeMove(Board board, GameController gameController){
		//minimax(board,depth,true);
		HashSet<Move> set = getAvailableMoves(board,gameController);
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
	static HashSet<Move> getAvailableMoves(Board board, GameController gameController){

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

	//TODO
	static int minimax(Board board, int depth, bool maximize){
		if(depth==0){
			return hueristic(board);
		}
		if(maximize){
			int bestValue = int.MaxValue;
		} else {

		}
		return -1;
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
