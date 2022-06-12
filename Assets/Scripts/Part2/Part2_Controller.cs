using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Part2_Controller : AStarController
{
    //simple class to hold agent data
    [Serializable]
    public class Agentinfo
    {
        public GameObject car;
        public GameObject start;
        public GameObject end;
        public int parcelCount;
    }

    // list of all the agents
    public List<Agentinfo> Cars = new List<Agentinfo>();
    //Dictionary of all the cars and information
    public Dictionary<AdvanceCar, Agentinfo> iCars = new Dictionary<AdvanceCar, Agentinfo>();
    private Dictionary<AdvanceCar, Connection> carConnections = new Dictionary<AdvanceCar, Connection>();


    [SerializeField]
    private Transform parentTransform = null;


    protected override void Start()
    {
        base.Start();

        foreach(Agentinfo info in Cars)
        {
            //error check
            if (info.start == null|| info.end == null)
            {
                continue;
            }

            GameObject Car = Instantiate(info.car, parentTransform);
            Part02_Car p2Car = Car.GetComponent<Part02_Car>();
            iCars.Add(p2Car, info);

            // Configure agents
            if (p2Car)
            {
                List<Connection> connetionPath = this.NavigateAStar(info.start, info.end);

                if (connetionPath != null)
                {
                    
                    p2Car.newConnection += NewConnection; //next connection
                    p2Car.pathEnd += PathEnd; //end of path
                    p2Car.SetPackages(info.parcelCount); //set parcel count
                    p2Car.DriveAlong(connetionPath); //drive along the path
                }
            }
        }
    }

    private void Update()
    {
        if (carConnections.Count > 0)
        {
           //for each car 
            foreach(KeyValuePair<AdvanceCar, Connection> thisKvp in carConnections)
            {
               
                KeyValuePair<AdvanceCar, Connection> matchingKvp = carConnections
                    .FirstOrDefault( kvp => kvp.Value.ToNode.name == thisKvp.Value.ToNode.name && kvp.Key.name != thisKvp.Key.name );
                AdvanceCar carOne = thisKvp.Key;
                AdvanceCar carTwo = matchingKvp.Key;

                //if car is waiting at a node
                if (carOne.Waiting)
                {
                    // add to waiting list
                    List<AdvanceCar> waitingCars = new List<AdvanceCar>();
                    bool isOnSameConnection = false;
                    
                    //for each car in wating list
                    foreach (KeyValuePair<AdvanceCar, Connection> checkKvp in carConnections)
                    {
                        if (thisKvp.Value.ToNode.name == checkKvp.Value.ToNode.name)
                        {
                            waitingCars.Add(thisKvp.Key);
                        }
                    }

                    //if more than one car waiting
                    if (waitingCars.Count > 1)
                    {
                        waitingCars[0].Movement();
                    }
                    else if (!isOnSameConnection)
                    {
                        carOne.Movement();
                    }
                }

              
                if ( matchingKvp.Key != null && matchingKvp.Value != null )
                {
                    if (!carOne.Waiting || carTwo.Waiting && thisKvp.Value.ToNode.name == matchingKvp.Value.ToNode.name)
                    {
                        if (carOne.Parcel.GetparcelCount > carTwo.Parcel.GetparcelCount)
                        {
                            //if car one is slower than two pause it
                            carOne.Pause();
                            Debug.Log($"Waiting'{carOne.name};");
                        }
                        //else
                        else
                        {
                            carTwo.Pause();
                            Debug.Log($"Waiting'{carTwo.name}'");
                        }
                    }
                }
            }
        }
    }


    // new connection event
    private void NewConnection(AdvanceCar car, Connection nextConnection)
    {
        // Update or add new connection to list
        if (carConnections.ContainsKey(car))
        {
            carConnections[car] = nextConnection;
        }
        else
        {
            carConnections.Add(car, nextConnection);
        }
    }

    //if the car reach end of the path call this 
    private void PathEnd(AdvanceCar car, GameObject arrivalWaypoint)
    {
        //get the car info
        KeyValuePair<AdvanceCar, Agentinfo> kvp = iCars.FirstOrDefault(x => x.Key.gameObject.name == car.gameObject.name);
        if (kvp.Key != null && kvp.Value != null)
        {
            Agentinfo info = kvp.Value;

            //when at end point deliver the parcels
            if (arrivalWaypoint == info.end)
            {
                StartCoroutine(Navigate(3f, car, info.end, info.start));
                car.Deliver();
            }
        }
    }

    // navigate the path using A star routine
    private IEnumerator Navigate(float seconds, AdvanceCar car, GameObject start, GameObject end)
    {
        yield return new WaitForSeconds(seconds);

       
        List<Connection> navPath = this.NavigateAStar(start, end);
        if (navPath != null)
        {
            car.DriveAlong(navPath);

            Debug.Log($"{car.gameObject.name}' returning to '{end.name}'");
        }
    }
}
