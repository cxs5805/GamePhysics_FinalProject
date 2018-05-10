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
    private ClickAndDrag[] draggable;

    // properties of the mass itself
    public float SPRING = 2.5f;
    public float SPRING_LENGTH = 2.0f;
    public float SPRING_FRICTION = 1.0f;
    private const float MAX_ACC = 1.0f;
    public float MAX_VEL = 10.0f;
    public float MIN_VEL = 0.5f;
    public Vector3 velocity;
    public Vector3 acceleration;
    public float friction;

    // gravity force constant
    public float gravity = -2.5f;
    public float impulseConstant = 20.0f;

    // camera attributes (for bouncing off walls)
    private GameObject cameraObject;
    private Camera camera;

    // Use this for initialization
    void Start ()
    {
        // as long as there are connections...
        if (adjacencyListObjects.Count > 0)
        {
            // ...get all of the mass scripts once at the start
            adjacencyList = new Mass[adjacencyListObjects.Count];
            draggable = new ClickAndDrag[adjacencyListObjects.Count];
            for (int i = 0; i < adjacencyList.Length; i++)
            {
                adjacencyList[i] = adjacencyListObjects[i].GetComponent<Mass>();
                draggable[i] = adjacencyListObjects[i].GetComponent<ClickAndDrag>();
            }
        }
        else
        {
            Debug.Log("The object " + name + " is not connected to a mass-spring system!");
            Debug.Break();
        }

        // random starting acceleration and velocity
        //acceleration = new Vector3(
        //    Random.Range(-1.41f * MAX_ACC, 1.41f * MAX_ACC),
        //    Random.Range(-1.41f * MAX_ACC, 1.41f * MAX_ACC));
        //velocity = new Vector3(
        //    Random.Range(-1.41f * MAX_VEL, 1.41f * MAX_VEL),
        //    Random.Range(-1.41f * MAX_VEL, 1.41f * MAX_VEL));
        acceleration = new Vector3(0.0f, gravity);

        // get the camera
        cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        camera = cameraObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update ()
    {
        // apply spring forces from all adjacent masses acting on the current mass
        //if (velocity.magnitude > MIN_VEL)
        //{
            for (int i = 0; i < adjacencyList.Length; i++)
            {
                if (draggable[i])
                    SpringForce(adjacencyList[i]);
            }
        //}

        // bounce off the walls
        // check collisions
        bool bounds = CheckBounds();
        if (bounds)
        {
            // respond
            Debug.Log("Bounce!");
        }

        // Euler
        // apply force of gravity
        velocity += acceleration * Time.deltaTime;

        // cap acceleration and velocity
        /*
        if (acceleration.magnitude >= MAX_ACC)
        {
            acceleration.Normalize();
            acceleration *= MAX_ACC;
        }
        //*/

        if (velocity.magnitude >= MAX_VEL)
        {
            velocity.Normalize();
            velocity *= MAX_VEL;
        }

        // round velocity down to zero below a certain amount
        if (velocity.magnitude <= MIN_VEL)
        {
            velocity *= 0.0f;
        }
        velocity *= friction;

        // update postion AFTER capping v and a
        transform.position += velocity * Time.deltaTime;
    }

    // algorithm adapted from TripleSpring flash example
    void SpringForce(Mass other)
    {
        // spring from here to the other object
        float dx = other.transform.position.x - transform.position.x;
        float dy = other.transform.position.y - transform.position.y;

        float angle = Mathf.Atan2(dy, dx);

        float targetX = other.transform.position.x - Mathf.Cos(angle) * SPRING_LENGTH;
        float targetY = other.transform.position.y - Mathf.Sin(angle) * SPRING_LENGTH;

        // force is applied directly as an impulse
        velocity.x += (targetX - transform.position.x) * SPRING;
        velocity.y += (targetY - transform.position.y) * SPRING;
        velocity.x *= SPRING_FRICTION;
        velocity.y *= SPRING_FRICTION;
    }

    bool CheckBounds()
    {
        // assume it's not colliding
        bool colliding = false;

        // get position in camera space
        Vector2 pos = camera.WorldToViewportPoint(transform.position);

        //Debug.Log("(" + pos.x + ", " + pos.y + ")");

        // check
        if (pos.x < 0 || pos.x > 1 || pos.y < 0 || pos.y > 1)
        {
            colliding = true;

            // since it is colliding, respond
            Bounce(pos, impulseConstant); 
        }


        return colliding;
    }

    void Bounce(Vector3 pos, float impulseConstant)
    {
        //*
        // apply impulse
        //impulseConstant = 4.0f;
        Vector3 impulse = Vector3.zero;

        // hard-coded values for screen bounds in global space
        Vector3 Max = new Vector3( 6.66f,  5.0f);
        Vector3 Min = new Vector3(-6.66f, -5.0f);

        // where to reposition the mass
        float newX = transform.position.x;
        float newY = transform.position.y;

        // in x
        if (pos.x < 0)
        {
            impulse.x = impulseConstant;
            newX = Min.x;
        }
        else if (pos.x > 1)
        {
            impulse.x = -impulseConstant;
            newX = Max.x;
        }

        // in y
        if (pos.y < 0)
        {
            impulse.y = impulseConstant;
            newY = Min.y;
        }
        else if (pos.y > 1)
        {
            impulse.y = -impulseConstant;
            newY = Max.y;
        }

        // reposition
        transform.position = new Vector3(newX, newY);

        // apply impulse(?)
        velocity += impulse * Time.deltaTime;
        //velocity = impulse;
        //*/
    }
}
