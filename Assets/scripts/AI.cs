using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class AI : MonoBehaviour {

	struct ScoreMove{
		public int score;
		public Move move;
		public ScoreMove(int score1, Move move1){
			score = score1;
			move = move1;
		}
	}

	static public int MAX_DEPTH = 3;
	static public int WIN_HUERISTIC = 10000;
	static public int COMPUTER_NUMBER = 2;
	static public int PLAYER_NUMBER = 1;

	public static Move makeMove(Board board){
        //If not the first move
        if (!board.isFirstMove())
        {
            Debug.Log("Not First Move");
            ScoreMove val = minimax(board, MAX_DEPTH, int.MinValue, int.MaxValue, true);
            Debug.Log(val.score);
            if (val.move != null)
            {
                return val.move;
            }
        }

		Debug.Log("Returning random Move");

		HashSet<Move> set = getAvailableMoves(board);
		int randNum = Random.Range(0,set.Count);
		int i=0;
		foreach(Move move in set){
			if(i==randNum){
//				Debug.Log("Move from AI"+move.ToString());
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
                if ((board.xPieces > 0 && board.turn == Board.PlayerTurn.X_TURN) || (board.oPieces > 0 && board.turn == Board.PlayerTurn.O_TURN))
                {//Check if enough pieces
                    if(board.positions[i][j] == 0){//Then check if space is free
					    set.Add(new Move(i,j,MoveType.PLACE));
                    }
				} else if(board.positions[i][j] == getPieceNumber(board.turn)) {
					ArrayList positions = BoardHelper.getInstance().getMovePositions(board,i,j);
					//can remove
					if(positions.Count == 0){
						set.Add(new Move(i,j,MoveType.REMOVE));
					} else {//add all move positions to set
						foreach(Vector2 vector in positions){
							set.Add(new Move(new Vector2(i,j),vector));
						}
					}
				}
			}
		}
		return set;
	}

    private static int getPieceNumber(Board.PlayerTurn playerTurn)
    {
        if (playerTurn == Board.PlayerTurn.X_TURN)
        {
            return 2;
        }
        else if (playerTurn == Board.PlayerTurn.O_TURN)
        {
            return 1;
        }
        return 0;
    }

	/**
	 * Minimax with alpha beta pruning.
	 * Returns hueristic
	 * */
	static ScoreMove minimax(Board board, int depth,int alpha, int beta, bool maximize){
		if(depth==0 ){//or terminal node (game over)
			//BoardHelper.getInstance().checkWin(x,y,board)
			return new ScoreMove(hueristic(board),null);
		}
        //update turn
        if (depth != MAX_DEPTH)
        {
            board.updateTurn();
        }
		if(maximize){
			ScoreMove bestValue = new ScoreMove(int.MinValue, null);
			foreach(Move move in getAvailableMoves(board)){
				Vector2[] clicks =  move.getClicks();
				BoardHelper helper = BoardHelper.getInstance();
				Board tempBoard = new Board(board);
                Vector2 lastMove;

				if(clicks.Length == 2){
					tempBoard = helper.simulateMove(tempBoard,clicks[0]);
					tempBoard = helper.simulateMove(tempBoard,clicks[1]);
                    lastMove = clicks[1];
				} else {//it is length 1
					tempBoard = helper.simulateMove(tempBoard,clicks[0]);
                    lastMove = clicks[0];
				}
                if (helper.checkWin(lastMove, tempBoard) == COMPUTER_NUMBER)
                {
                    Debug.Log("Max: " + move);
                    return new ScoreMove(WIN_HUERISTIC, move);
                }

				ScoreMove val = minimax(tempBoard,depth-1,alpha,beta,!maximize);
				if(bestValue.score < val.score ){
					bestValue = val;
                    bestValue.move = move;
				}
				alpha = Mathf.Max(alpha,val.score);
				if(beta <= alpha){
//					break;
				}
			}
			return bestValue;
		} else {
			ScoreMove bestValue = new ScoreMove(int.MaxValue,null);
			foreach(Move move in getAvailableMoves(board)){
				Vector2[] clicks =  move.getClicks();
				BoardHelper helper = BoardHelper.getInstance();
				Board tempBoard = new Board(board);
                Vector2 lastMove;
				if(clicks.Length == 2){
					tempBoard = helper.simulateMove(tempBoard,clicks[0]);
					tempBoard = helper.simulateMove(tempBoard,clicks[1]);
                    lastMove = clicks[1];
				} else {
					tempBoard = helper.simulateMove(tempBoard,clicks[0]);
                    lastMove = clicks[0];
				}
                if (helper.checkWin(lastMove, tempBoard) == PLAYER_NUMBER)
                {
                    Debug.Log("Min: " + move);
                    return new ScoreMove(-1 * WIN_HUERISTIC, move);
                }
				//update turn
				//tempBoard.updateTurn();
				ScoreMove val = minimax(tempBoard,depth-1,alpha,beta,!maximize);
				if(bestValue.score > val.score){
					bestValue = val;
                    bestValue.move = move;

				}
				beta = Mathf.Min(beta,val.score);
				if(beta <= alpha){
		//			break;
				}
			}
			return bestValue;
		}
	}

	static int hueristic(Board board){
        float h = 0;
        int pieceNumber = getPieceNumber(board.turn);
        if (pieceNumber == board.positions[1][1])
        {
            h += board.positions[1][1];
        }
        if (pieceNumber == board.positions[1][2])
        {
            h += board.positions[1][2];
        }
        if (pieceNumber == board.positions[2][1])
        {
            h += board.positions[2][1];
        }
        if (pieceNumber == board.positions[2][2])
        {
            h += board.positions[2][2];
        }
        h = Mathf.Pow(h / pieceNumber, 2);
        if (board.turn == Board.PlayerTurn.O_TURN) {
            h = h + 100 * (board.oPieces - board.xPieces);
            h = -1*h;
        }
        else
        {
            h = h + 100 * (board.xPieces - board.oPieces);
        }
        return (int)h;
        /*for (int i = 0; i < board.positions.Length; i++)
        {
            for (int j = 0; j < board.positions[i].Length; j++)
            {

            }
        }*/
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
