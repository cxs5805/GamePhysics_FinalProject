using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mass : MonoBehaviour
{
    // This script defines the mass in a mass-spring system.

    // The actual implementation of the spring system is 
    // done using adjacency lists for each mass in the system.
    // In other words, the springs do not exist independent of
    // the masses themselves from an object-oriented standpoint. 
    public List<GameObject> adjacencyListObjects;
    private Mass[] adjacencyList;

    // properties of the mass itself
    private const float SPRING = 0.5f;
    private const float SPRING_LENGTH = 1.0f;
    private const float FRICTION = 1.0f;
    private const float MAX_VEL = 1.0f;
    public Vector3 velocity;

	// Use this for initialization
	void Start ()
    {
        // as long as there are connections...
        if (adjacencyListObjects.Count > 0)
        {
            // ...get all of the mass scripts once at the start
            adjacencyList = new Mass[adjacencyListObjects.Count];
            for (int i = 0; i < adjacencyList.Length; i++)
            {
                adjacencyList[i] = adjacencyListObjects[i].GetComponent<Mass>();
            }
        }
        else
        {
            Debug.Log("The object " + name + " is not connected to a mass-spring system!");
            Debug.Break();
        }

        // random starting velocity
        velocity = new Vector3(
            Random.Range(-1.41f * MAX_VEL, 1.41f * MAX_VEL),
            Random.Range(-1.41f * MAX_VEL, 1.41f * MAX_VEL));
	}
	
	// Update is called once per frame
	void Update ()
    {
        // apply spring forces from all adjacent masses acting on the current mass
        /*
        for (int i = 0; i < adjacencyList.Length; i++)
        {
            springForce(adjacencyList[i]);
        }
        //*/

        // Euler
        transform.position += velocity * Time.deltaTime;
    }

    void springForce(Mass other)
    {
        // spring from here to the other object
        float dx = other.transform.position.x - transform.position.x;
        float dy = other.transform.position.y - transform.position.y;

        float angle = Mathf.Atan2(dy, dx);

        float targetX = other.transform.position.x - Mathf.Cos(angle) * SPRING_LENGTH;
        float targetY = other.transform.position.y - Mathf.Sin(angle) * SPRING_LENGTH;

        velocity.x += (targetX - transform.position.x) * SPRING;
        velocity.y += (targetY - transform.position.y) * SPRING;
        velocity.x *= FRICTION;
        velocity.y *= FRICTION;
    }
}
