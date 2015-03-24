using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour {

	public int depth = 2;

	public static Vector2 makeMove(int[][] board, int pieces, GameController gameController){
		//minimax(board,depth,true);
		HashSet<Vector2> set = getAvailableMoves(board,pieces,gameController);
		int randNum = Random.Range(0,set.Count);
		int i=0;
		foreach(Vector2 vector in set){
			if(i==randNum){
				return vector;
			}
			i++;
		}
		throw new UnityException("AHH BADDDDD");
	}
	/*
	 * Returns ArrayList of Vector2 of all available moves
	 * */
	static HashSet<Vector2> getAvailableMoves(int[][]board, int pieces, GameController gameController){
		HashSet<Vector2> set = new HashSet<Vector2>();
		for(int i=0; i<board.Length;i++){
			for(int j=0; j<board[i].Length;j++){
				if(pieces > 0 && board[i][j] == 0){
					set.Add(new Vector2(i,j));
					//TODO make this not always 2
				} else if(board[i][j] == 2) {
					ArrayList positions = gameController.getMovePositions(i,j);
					//can remove
					if(positions.Count == 0){
						set.Add(new Vector2(i,j));
					} else {//add all move positions to set
						foreach(Vector2 vector in positions){
							set.Add(vector);
						}
					}
				}
			}
		}
		return set;
	}

	static int minimax(int[][] board, int depth, bool maximize){
		if(depth==0){
			return hueristic(board);
		}
		if(maximize){
			int bestValue = int.MaxValue;
		} else {

		}
		return -1;
	}

	static int hueristic(int[][]board){
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
