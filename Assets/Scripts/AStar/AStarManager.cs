using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarManager
{
    private AStar AStar = new AStar();

    private Graph aGraph = new Graph();

    private Heuristic aHeuristic = new Heuristic();

    public AStarManager() { 
    
    }

    public void AddConnection(Connection connection) {
        aGraph.Addconnection(connection);
    }

    public List<Connection> pathfindAstar(GameObject start, GameObject end) {
        return AStar.pathfinderAstar(aGraph, start, end, aHeuristic);
    }
}
