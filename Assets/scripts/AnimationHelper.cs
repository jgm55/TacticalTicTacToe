using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class AnimationHelper : MonoBehaviour {

    public static Vector3 virtualMousePos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void doMove(Move move)
    {
        BlockController block1 = null, block2 = null;
        foreach (BlockController block in FindObjectsOfType<BlockController>())
        {
            if (block.x == move.getPositionOne().y && block.y == move.getPositionOne().x)
            {
                block1 = block;
            }
            if (move.getPositionTwo() != null)
            {
                if (block.x == move.getPositionTwo().y && block.y == move.getPositionTwo().x)
                {
                    block2 = block;
                }
            }
        }
        PieceController pieceAtBlock1 = null;
        if (block1 != null)
        {
            virtualMousePos = block1.transform.position;
            RaycastHit[] hits = Physics.RaycastAll(virtualMousePos + new Vector3(0,0,10), Vector3.back);
            foreach (RaycastHit hit in hits)
            {
                PieceController piece = hit.collider.gameObject.GetComponent<PieceController>();
                if (piece != null)
                {
                    pieceAtBlock1 = piece;
                }
            }
        }
        else
        {
            throw new UnityException("block1 is null!");
        }
        if (move.type == MoveType.REMOVE)
        {
            if (pieceAtBlock1 != null)
            {
                pieceAtBlock1.returnToStart();
            }
            else
            {
                throw new UnityException("pieceAtBlock1 is null!");
            }
        }
        else if (move.type == MoveType.PLACE)
        {
            foreach(PieceController piece in FindObjectsOfType<PieceController>()){
                if(piece.state == PieceController.PieceState.START && piece.blockType == FindObjectOfType<GameController>().turnToBlockType()){
                    piece.goToBlock(block1);
                    break;
                }
            }
        }
        else if(move.type == MoveType.MOVE){
            pieceAtBlock1.goToBlock(block2);
        }
    }

    public static RaycastHit2D[] getRayCastFromScreen(Vector3 pos)
    {
        Vector2 camPos = Camera.main.ScreenToWorldPoint(pos);
        return Physics2D.RaycastAll(camPos, Vector2.zero);
    }
}
