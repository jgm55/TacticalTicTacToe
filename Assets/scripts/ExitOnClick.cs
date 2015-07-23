using UnityEngine;
using System.Collections;

public class ExitOnClick : MonoBehaviour {

    void Update()
    {
        RaycastHit2D[] hits = AnimationHelper.getRayCastFromScreen(Input.mousePosition);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject == this)
            {
                OnMouseDown();
            }
        }
    }
    void OnMouseDown()
    {
        Debug.Log("QUITTING");
        Application.Quit();
    }
}
