using UnityEngine;
using System.Collections;

public class SwipeController : MonoBehaviour {

    float sensitivity = 02f;
    private Vector3 mouseReference;
    private Vector3 mouseOffset;
    private Vector3 location;
    private bool isSliding;
    private Vector3 clickDrag;

    private bool right;
    //private GameObject parentObject;

    void Start()
    {
        location = Vector3.zero;
    }

    void Update()
    {
        //Click and drag on (hidden) cylinder to ring and fish.

        clickDrag = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        if (isSliding)
        {
            // offset
            mouseOffset = (Input.mousePosition - mouseReference);

            GetComponent<Rigidbody2D>().AddForce(new Vector3(mouseOffset.x * sensitivity, 0));

            // move
            //gameObject.transform.position = location;

            // store mouse
            mouseReference = Input.mousePosition;
        }
    }

    void OnMouseDown()
    {
        // sliding flag
        isSliding = true;

        // store mouse
        mouseReference = Input.mousePosition;
    }

    void OnMouseUp()
    {
        // rotating flag
        isSliding = false;
    }
    /*
    void RightSwipe()
    {
        Debug.Log("Right Swipe");
        //load next gameObject
        if (slideNumber >= tutorialSlides.Length)
        {
            Application.LoadLevel(startScreen);
        }
        else
        {
            slideNumber++;

        }
    }

    void LeftSwipe()
    {
        Debug.Log("left Swipe");
        if(slideNumber > 0){
            slideNumber--;

        }
        else
        {
            //do a wiggle or something
        }
    }*/
}
