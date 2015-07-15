using UnityEngine;
using System.Collections;

public class SwipeController : MonoBehaviour {
    private Vector3 lastTouchPos = new Vector3(0, 0, 0);
    public string nextLevel;
    public string previousLevel = "";

    // Use this for initialization
    void Start()
    {
        Debug.Log("Start");

        Debug.Log("Start2");

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonUp(0) && !lastTouchPos.Equals(new Vector3(0, 0, 0)))
        {
            if (lastTouchPos.x > Input.mousePosition.x)
            {
                RightSwipe();
            }
            else if (lastTouchPos.x < Input.mousePosition.x)
            {
                LeftSwipe();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            lastTouchPos = Input.mousePosition;
        }
    }

    void RightSwipe()
    {
        Debug.Log("Right Swipe");
        Application.LoadLevel(nextLevel);
    }

    void LeftSwipe()
    {
        Debug.Log("left Swipe");

        if (previousLevel != "")
        {
            Application.LoadLevel(previousLevel);
        }
    }
    /*float sensitivity = 100f;
    //private Vector3 mouseOffset;
    private Vector3 location, previousLocation;
    private bool isSliding;
    private Vector3 clickDrag;
    private int slideNumber = 0;

    private float totalDistanceClick = 0f;

    public GameObject[] tutorialSlides= new GameObject[2];

    private bool right;
    //private GameObject parentObject;

    void Start()
    {
        location = tutorialSlides[0].transform.position;
        previousLocation = tutorialSlides[0].transform.position;
    }

    void Update()
    {
        //Click and drag on (hidden) cylinder to ring and fish.

        clickDrag = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (isSliding)
        {
            // offset
            //mouseOffset = (Input.mousePosition - mouseReference);
            float xMove = Mathf.Lerp(location.x, clickDrag.x * sensitivity, Time.deltaTime);
            totalDistanceClick += xMove;

            location = Vector3.Lerp(location, new Vector3(xMove, location.y, location.z), Time.deltaTime);

            // move
            gameObject.transform.position = location;

            if (slideNumber + 1 < tutorialSlides.Length &&
                totalDistanceClick > Mathf.Abs(tutorialSlides[slideNumber + 1].transform.position.x - previousLocation.x) / 2)
            {
                slideNumber++;
                previousLocation = tutorialSlides[slideNumber].transform.position;
            } else if (slideNumber - 1 >= 0 &&
              totalDistanceClick > Mathf.Abs(tutorialSlides[slideNumber - 1].transform.position.x - previousLocation.x) / 2)
            {
                slideNumber--;
                previousLocation = tutorialSlides[slideNumber].transform.position;
            }
        }
        else
        {
            location = Vector3.Lerp(location, previousLocation, Time.deltaTime);
            //Move back to previous position
            gameObject.transform.position = location;
        }
    }

    void OnMouseDown()
    {
        // sliding flag
        isSliding = true;
    }

    void OnMouseUp()
    {
        // rotating flag
        isSliding = false;

        totalDistanceClick = 0f;
    }*/
}
