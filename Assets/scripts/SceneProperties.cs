using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class SceneProperties : MonoBehaviour {

	public static bool aiPlaying = false;
    public static bool heldPiece = false;

    public static Board.PlayerTurn aiTurn = Board.PlayerTurn.X_TURN;
}
