using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // note: right now, I'm assuming just one softbody in the simulation
    // if time permits, try two

    // need the parent object of all the point masses
    // this parent object is itself the softbody
    // just a public variable
    // ADD THE OBJECT IN THE EDITOR
    public GameObject softbody;
    private Mass[] masses;

    // hard-coded constant values for bounds of screen
    private const float X = 6.66f;
    private const float Y = 5.0f;

    // Use this for initialization
    void Start()
    {
        // GET ALL THE CHILDRENS' RELEVANT SCRIPTS
        masses = softbody.GetComponentsInChildren<Mass>();
        
        // give them random velocities in init
        //Init();
    }

    // Update is called once per frame
    void Update()
    {
        // read user input to restart simulation
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Init();
        }
    }

    void Init()
    {
        // this line may be unneeded.
        // give the softbody a random position on the screen
        //softbody.transform.position = new Vector3(Random.Range(-X, X), Random.Range(-Y, Y));
        Vector3 COM = new Vector3(Random.Range(-X, X), Random.Range(-Y, Y));
        float radius = masses[0].SPRING_LENGTH * 1.1f;

        // give point masses random positions and velocities
        for (int i = 0; i < masses.Length; i++)
        {
            masses[i].transform.position = new Vector3(
                COM.x + Random.Range(-radius, radius),
                COM.y + Random.Range(-radius, radius));

            masses[i].velocity = new Vector3(
                
                // old way
                //Random.Range(-1.41f * masses[i].MAX_VEL, 1.41f * masses[i].MAX_VEL),
                //Random.Range(-1.41f * masses[i].MAX_VEL, 1.41f * masses[i].MAX_VEL)
                
                // new way
                Random.Range(-1.0f, 1.0f),
                Random.Range(-1.0f, 1.0f) 
                );
        }

    }
}
