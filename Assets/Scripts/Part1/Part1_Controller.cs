using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

public class Part1_Controller : AStarController
{
    //start way point
    public GameObject StartWaypoint = null;

    //end point
    public GameObject EndWaypoint = null;

    // car object
    public GameObject CarObj = null;


    private SimpleCar car;

    private bool m_returningtoMall = false;

    //car wait time
    private const float WAIT_SEC = 3f;

    protected override void Start()
    {
        base.Start();

        if (!EndWaypoint || !StartWaypoint) {
            Debug.LogError("No start or destination way points...> try again");
            return;
        }

        //find the path to end way point
        List<Connection> connectionPath = this.NavigateAStar(StartWaypoint, EndWaypoint);
        if (connectionPath == null)
        {
            Debug.LogError("Path finding error");
            return;
        }

        //ask the car to drive along the path
        if (CarObj != null)
        {
            car = CarObj.GetComponent<SimpleCar>();
            if (car)
            {
                car.OnPathEnd += OnReachedEndPath;

                car.DriveAlong(connectionPath);
            }
        }
        else
        {

            return;
        }
    }

    void OnReachedEndPath()
    {
        if (m_returningtoMall)
        {
            
            Debug.Log("Returned");
        }
        else
        {
            // Reached delivery drop off location, wait and then return to start position
            StartCoroutine(WaitNav(WAIT_SEC, EndWaypoint, StartWaypoint));

            // Drop off package
            car.DeliverPackage();

            //on return path
            m_returningtoMall = true;

            Debug.Log("Reached destination");
        }
    }

    //return path for the Del car
    private IEnumerator WaitNav(float seconds, GameObject start, GameObject end)
    {
        yield return new WaitForSeconds(seconds);

        //new path
        List<Connection> returnPath = this.NavigateAStar(start, end);
        car.DriveAlong(returnPath);
    }

}
