using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarController : MonoBehaviour
{
    // The AStarManager 
    private AStarManager m_astarManager = new AStarManager();
    
    // All way points 
    protected List<GameObject> m_allWaypoints = new List<GameObject>();

    protected virtual void Start()
    {
        Init();
    }

    private void Init()
    {
        GameObject[] Waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        foreach (GameObject waypoint in Waypoints)
        {
            WaypointCon tempCon = waypoint.GetComponent<WaypointCon>();
            if (tempCon)
                m_allWaypoints.Add(waypoint);
        }

        
        foreach (GameObject waypoint in m_allWaypoints)
        {
            WaypointCon tempCon = waypoint.GetComponent<WaypointCon>();
            foreach (GameObject waypointConNode in tempCon.Connections)
            {
                Connection aConnection = new Connection();
                aConnection.FromNode = waypoint;
                aConnection.ToNode = waypointConNode;

                m_astarManager.AddConnection(aConnection);
            }
        }
    }

    // navigate to a way point using A star
    protected List<Connection> NavigateAStar(GameObject start, GameObject end)
    {
        // error cheack
        if (start == null || end == null)
        {
            return null;
        }

    
        List<Connection> connectionPath = m_astarManager.pathfindAstar(start, end);

        if (connectionPath == null || connectionPath != null && connectionPath.Count <= 0)
        {
            return null;
        }
                
        return connectionPath; // return path
    }
}
