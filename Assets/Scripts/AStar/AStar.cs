using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    public AStar() { 
    
    }

    public List<Connection> pathfinderAstar(Graph aGraph, GameObject start, GameObject end, Heuristic myHeuristic) {
        NodeRecord StartRecord = new NodeRecord();
        StartRecord.Node = start;
        StartRecord.Connection = null;
        StartRecord.CostSoFar = 0;
        StartRecord.EstimatedTotalCost = myHeuristic.Estimate(start, end);

        PathfindingList OpenList = new PathfindingList();
        PathfindingList CloseList = new PathfindingList();

        OpenList.AddNodeRecod(StartRecord);

        NodeRecord CurrentRecord = null;
        List<Connection> connections;

        while (OpenList.Getsize() > 0) {
            CurrentRecord = OpenList.GetSmallestElement();

            if (CurrentRecord.Node.Equals(end)) {
                break;
            }

            connections = aGraph.GetConnections(CurrentRecord.Node);

            GameObject EndNode;
            float EndNodeCost;
            NodeRecord EndNodeRecord;
            float EndNodeHeuristic;

            foreach (Connection aConnection in connections) {
                EndNode = aConnection.ToNode;
                EndNodeCost = CurrentRecord.CostSoFar + aConnection.Cost;

                if (CloseList.Contains(EndNode))
                {
                    EndNodeRecord = CloseList.Find(EndNode);

                    if (EndNodeRecord.CostSoFar <= EndNodeCost)
                    {
                        continue;
                    }

                    CloseList.RemoveNodeRecode(EndNodeRecord);

                    EndNodeHeuristic = EndNodeRecord.EstimatedTotalCost - EndNodeRecord.CostSoFar;
                }
                else if (OpenList.Contains(EndNode))
                {
                    EndNodeRecord = OpenList.Find(EndNode);

                    if (EndNodeRecord.CostSoFar <= EndNodeCost)
                    {
                        continue;
                    }
                    EndNodeHeuristic = EndNodeRecord.EstimatedTotalCost - EndNodeRecord.CostSoFar;
                }
                else {
                    EndNodeRecord = new NodeRecord();
                    EndNodeRecord.Node = EndNode;

                    EndNodeHeuristic = myHeuristic.Estimate(EndNode, end);
                }
                EndNodeRecord.CostSoFar = EndNodeCost;
                EndNodeRecord.Connection = aConnection;
                EndNodeRecord.EstimatedTotalCost = EndNodeCost + EndNodeHeuristic;

                if (!(OpenList.Contains(EndNode))) {
                    OpenList.AddNodeRecod(EndNodeRecord);
                }
            }
            OpenList.RemoveNodeRecode(CurrentRecord);
            CloseList.AddNodeRecod(CurrentRecord);
        }
        List<Connection> tempList = new List<Connection>();
        if (!CurrentRecord.Node.Equals(end))
        {
            return tempList;
        }
        else {
            while (!CurrentRecord.Node.Equals(start)) {
                tempList.Add(CurrentRecord.Connection);
                CurrentRecord = CloseList.Find(CurrentRecord.Connection.FromNode);
            }
            List<Connection> tempList2 = new List<Connection>();
            for (int i = (tempList.Count - 1); i >= 0; i--) {
                tempList2.Add(tempList[i]);
            }

            return tempList2;
        }
    }
}
