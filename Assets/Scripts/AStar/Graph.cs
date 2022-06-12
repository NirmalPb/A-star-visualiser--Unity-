using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    private List<Connection> WaypointConnections = new List<Connection>();

    public Graph() { 
    
    }

    public void Addconnection(Connection aConnection) {
        WaypointConnections.Add(aConnection);
    }

    public List<Connection> GetConnections(GameObject FromNode) {
        List<Connection> TmpConnections = new List<Connection>();
        foreach (Connection aConnection in WaypointConnections) {
            if (aConnection.FromNode.Equals(FromNode)) {
                TmpConnections.Add(aConnection);
            }
        }
        return TmpConnections;
    }
}
