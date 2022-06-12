using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Part3_Controller : ACOController
{
    //sma;l class to hold agent info
    [Serializable]
    public class Info
    {
        
        public GameObject car; //car
        public int parcelcount; //num of parcels
        public GameObject start; //start
        public List<GameObject> endGoals; //end goals
    }

    //small class to hold ACO info
    [Serializable]
    public class ACOConfig {
        public float Alpha = 1.0f;
        public float Beta = 0.0001f;
        public float EvaporationFactor = 0.5f;
        public float Q = 0.0005f;
        public int MaxIterations = 150;
        public int Ants = 50;
        public int MaxPathLength = 15;

    }
    // info of all the cars
    public List<Info> AgentsInfo = new List<Info>();

    //acoConfig information
    public ACOConfig ACO_Config = new ACOConfig();

    //list of all the agents
    private List<GameObject> Agents = new List<GameObject>();

    //list of trucks and path info
    private Dictionary<ACOCar, pathinfo> Connections = new Dictionary<ACOCar, pathinfo>();
    [SerializeField, Tooltip("Parent transform to instantiate new agents under")]
    private Transform Parent = null;

    protected override void Start() {
        base.Start();

        //get ACO values from unity
        if (ACO_Config != null)
        {
            this.ConfigACOCON(ACO_Config.Alpha, ACO_Config.Beta, 
                ACO_Config.EvaporationFactor, ACO_Config.Q);
        }

        //config each agents and find path
        foreach (Info carInfo in AgentsInfo)
        {
            //create a  list of all the agent in the scene 
            GameObject inst = Instantiate(carInfo.car, Parent);
            Agents.Add(inst);
            ACOCar acoCar = inst.GetComponent<ACOCar>();

            //config ACO path
            if (acoCar)
            {
                acoCar.Parcel.AddPackages(carInfo.parcelcount); // add parcels
                List<ACOConnection> goalACOConnections = GoalsAndRoutes(carInfo.endGoals); //create ACO path
                GameObject FirstNode = GetClosestNode(goalACOConnections, carInfo.start.transform.position); //create goal location from close to farthest
                List<ACOConnection> route = this.GenACOPath(ACO_Config.MaxIterations, ACO_Config.Ants, m_allWaypoints.ToArray(), goalACOConnections, FirstNode, ACO_Config.MaxPathLength);

                //calc the path back to start
                GameObject LastNode = route.LastOrDefault().ToNode;

                //create a A* path with ACO path using a* algoritham
                List<Connection> StarttoEnd = this.NavigateAStar(carInfo.start, FirstNode);
                List<Connection> EndtoStart = this.NavigateAStar(LastNode, carInfo.start);

                //car event listeners
                acoCar.NewConnection += NewConnection;
                acoCar.ReachedGoal += OnGoal;
            }
            else {
                Debug.Log("Error with Agents or pathfinder");
            }
        }
    }

    private void Update()
    {
        if (Connections.Count > 0) {
            //for each car 
            foreach (KeyValuePair<ACOCar, pathinfo> thisKvp in Connections)
            {

                KeyValuePair<ACOCar, pathinfo> matchingKvp = Connections.FirstOrDefault(kvp => kvp.Value.To.name == thisKvp.Value.To.name && kvp.Key.name != thisKvp.Key.name);
                ACOCar carOne = thisKvp.Key;
                ACOCar carTwo = matchingKvp.Key;

                //if car is waiting at a node
                if (carOne.Waiting)
                {
                    // add to waiting list
                    List<ACOCar> waitingCars = new List<ACOCar>();
                    bool isOnSameConnection = false;

                    //for each car in waiting list
                    foreach (KeyValuePair<ACOCar, pathinfo> checkKvp in Connections)
                    {
                        if (thisKvp.Value.To.name == checkKvp.Value.To.name)
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

                //if match is not null
                if (matchingKvp.Key != null && matchingKvp.Value != null)
                {
                   
                }
            }
        }
    }

    //deliver parcel at the goal end
    private void OnGoal(ACOCar car, GameObject acoGoal)
    {
        car.Deliver();
    }

    //when car reach a new conncetion node
    private void NewConnection(ACOCar Car, pathinfo target)
    {
    
        if (Connections.ContainsKey(Car))
        {
            Connections[Car] = target;
        }
        else
        {
           Connections.Add(Car, target);
        }
    }


    //return the goal connections
    private List<ACOConnection> GoalsAndRoutes(List<GameObject> goals)
    {
        List<ACOConnection> goalConnections = new List<ACOConnection>();
        foreach (GameObject goal in goals)
        {
            foreach (GameObject j in goals)
            {
                if (goal != j)
                {
                    // Set ACO From and To
                    ACOConnection acoConnection = new ACOConnection();
                    acoConnection.SetConnection(goal, j, 1.0f);

                    //navigate A* and set path
                    acoConnection.SetConnection(acoConnection.FromNode, acoConnection.ToNode, DefaultPheromone);
                    goalConnections.Add(acoConnection);
                }
            }
        }
        return goalConnections;
    }


    //returns the closest next node to the current node
    private GameObject GetClosestNode(List<ACOConnection> Connections, Vector3 position)
    {
        GameObject closest = null;
        float lastDist = 10000f;
        foreach (ACOConnection conn in Connections)
        {
            float distance = Vector3.Distance(conn.FromNode.transform.position, position);
            if (distance < lastDist)
            {
                closest = conn.FromNode;
                lastDist = distance;
            }
        }
        return closest;
    }
}
