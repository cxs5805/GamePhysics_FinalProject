using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAndDrag : MonoBehaviour
{
    // This script handles the logic for clicking and dragging
    // a 2D object. 

    private bool dragging;

	// Use this for initialization
	void Start ()
    {
        dragging = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // https://answers.unity.com/questions/1161704/how-to-move-in-game-2d-game-object-with-mouse.html
        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }

        if (dragging)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
        }
    }

    private void OnMouseDown()
    {
        dragging = true;
    }
}
