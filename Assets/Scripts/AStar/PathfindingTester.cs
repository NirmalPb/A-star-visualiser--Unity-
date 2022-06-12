using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingTester : MonoBehaviour
{
    private AStarManager AStarManager = new AStarManager();

    private List<GameObject> Waypoints = new List<GameObject>();

    private List<Connection> connectionArray = new List<Connection>();

    [SerializeField]
    private GameObject start;
    [SerializeField]
    private GameObject end;
    // Start is called before the first frame update

    Vector3 Offset = new Vector3(0, 0.3f, 0);
    void Start()
    {
        if (start == null || end == null) {
            Debug.Log("No start or end WayPoints");
            return;
        }

        GameObject[] gameObjectsWithWaypointTag;
        gameObjectsWithWaypointTag = GameObject.FindGameObjectsWithTag("Waypoint");

        foreach (GameObject waypoint in gameObjectsWithWaypointTag) {
            WaypointCon tmpwaypointCon = waypoint.GetComponent<WaypointCon>();
            if (tmpwaypointCon) {
                Waypoints.Add(waypoint);
            }
        }

        foreach (GameObject waypoint in Waypoints) {
            WaypointCon tmpWaypointCon = waypoint.GetComponent<WaypointCon>();
            foreach (GameObject WaypointConNode in tmpWaypointCon.Connections) {
                Connection aConnection = new Connection();
                aConnection.FromNode = waypoint;
                aConnection.ToNode = WaypointConNode;

                AStarManager.AddConnection(aConnection);
            }
        }

        connectionArray = AStarManager.pathfindAstar(start, end);
        
    }

    void OnDrawGizmos() {
        foreach (Connection aConnection in connectionArray) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine((aConnection.FromNode.transform.position + Offset), 
                (aConnection.ToNode.transform.position + Offset));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
