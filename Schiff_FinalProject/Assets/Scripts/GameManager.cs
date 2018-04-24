using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cameraObject;
    private Camera camera;

	// Use this for initialization
	void Start ()
    {
        camera = cameraObject.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //if (Input.GetMouseButton(0))  
            CastRay();
	}

    void CastRay ()
    {
        // based on
        // https://gamedev.stackexchange.com/questions/82928/2d-detect-mouse-click-on-object-with-no-script-attached
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 100))
        {
            Debug.DrawLine(ray.origin, hit.point);
            Debug.Log("!!!!!");
            Debug.Log("Hit object: " + hit.collider.gameObject.name);
        }
        Debug.Log("(" + Input.mousePosition.x + ", " + Input.mousePosition.y + ")");
    }
}
