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

namespace AssemblyCSharp
{
	public class Board
	{

		public enum PlayerTurn{X_TURN, O_TURN, NUETRAL, O_WINS, X_WINS};

		/**
		 * 0 is empty, 1 is O, 2 is x
		 * */
		public int[][]positions;
		public int oPieces;
		public int xPieces;
		public PlayerTurn turn;

        public GameController.BlockState getStateOfSquare(int x, int y)
        {
            switch (positions[x][y])
            {
                case 1:
                    return GameController.BlockState.O;
                case 2:
                    return GameController.BlockState.X;
                default:
                    return GameController.BlockState.NUETRAL;
            }
        }

		public Board (int boardSize, int piecesStart, PlayerTurn startTurn)
		{
			oPieces = piecesStart;
			xPieces = piecesStart;

			turn = startTurn;

			positions = new int[boardSize][];
			for(int i=0; i<boardSize; i++){
				int[] temp = new int[boardSize];
				for(int j=0; j<boardSize; j++){
					temp[j] = 0;
				}
				positions[i] = temp;
			}
		}

		public Board(Board other){
			oPieces = other.oPieces;
			xPieces = other.xPieces;

			turn = other.turn;

			int boardSize = other.positions.Length;
			positions = new int[boardSize][];
			for(int i=0; i<boardSize; i++){
				int[] temp = new int[boardSize];
				for(int j=0; j<boardSize; j++){
					temp[j] = other.positions[i][j];
				}
				positions[i] = temp;
			}
		}

		public void updateTurn(){
			if(turn == PlayerTurn.O_TURN){
				turn = PlayerTurn.X_TURN;
			} else if(turn == PlayerTurn.X_TURN){
				turn = PlayerTurn.O_TURN;
			}
		}
	}
}

