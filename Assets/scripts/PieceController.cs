using UnityEngine;
using System.Collections;

public class PieceController : MonoBehaviour {

    public BlockController.BlockState blockType = BlockController.BlockState.X;

    public enum PieceState { HELD,PLACED,RETURNING,START}

    PieceState state = PieceState.START;

    Vector3 startPos;
    Vector3 placePos = Vector3.zero;
    Vector3 returnPlace;

    float count = 0f;
    float duration = .5f;
    Vector3 moveDist;

	// Use this for initialization
	void Start () {
        startPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (state == PieceState.RETURNING)
        {
            this.transform.position += Time.deltaTime* moveDist;
            if (count >= duration)
            {
                state = PieceState.RETURNING;
                count = 0f;
                transform.position = returnPlace;
            }
            count += Time.deltaTime;
        }
        if (state == PieceState.HELD)
        {
            this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 pos = this.transform.position;
            this.transform.position = new Vector3(pos.x, pos.y, startPos.z);
        }
	}

    void OnMouseDown()
    {
        SceneProperties.heldPiece = true;
        state = PieceState.HELD;
    }

    void OnMouseUp()
    {
        //find if block is hit:
        Vector2 camPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        SceneProperties.heldPiece = false;
        state = PieceState.RETURNING;

        //TODO fix this for block controller
        RaycastHit2D[] hits = Physics2D.RaycastAll(camPos, Vector2.zero);
        if (hits.Length == 0)
        {
            //No hit found.
            state = PieceState.RETURNING;
            BlockController[] blocks = FindObjectsOfType<BlockController>();
            foreach(BlockController block in blocks){
                if(block.transform.position == placePos && !block.canMove()){
                    block.clickSquare(blockType);
                    returnPlace = startPos;
                    placePos = Vector3.zero;
                    break;
                }
            }            
        } else if(hits.Length == 3){
            //Already a piece placed
            state = PieceState.RETURNING;
        }
        else {
            foreach (RaycastHit2D hit in hits)
            {
                Debug.Log(hit.collider.gameObject);
                if (hit.collider.gameObject != this.gameObject)
                {
                    BlockController block = hit.collider.gameObject.GetComponent<BlockController>();
                    if (block != null)
                    {
                        if (block.clickSquare(blockType)) {
                            this.transform.position = block.transform.position;
                            state = PieceState.PLACED;
                            placePos = transform.position;
                            returnPlace = placePos;
                        }
                    }
                }
            }
        }

        Vector3 pos = transform.position;
        moveDist = new Vector3((startPos.x - pos.x)/ duration, (startPos.y - pos.y )/ duration,0);
        //tween to start position
        //this.transform.position = startPos;
        //iTween.MoveTo(this.gameObject, iTween.Hash("position", 2, "easeType", "easeInOutExpo", "time", .3));
        //iTween.moveTo(startPos);
    }
}
