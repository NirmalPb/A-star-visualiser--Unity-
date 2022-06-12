using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingList
{
    private List<NodeRecord> NodeRecordList = new List<NodeRecord>();

    public PathfindingList() { 
    
    }

    public void AddNodeRecod(NodeRecord NodeRecord) {
        NodeRecordList.Add(NodeRecord);
    }

    public void RemoveNodeRecode(NodeRecord NodeRecord) {
        NodeRecordList.Remove(NodeRecord);
    }

    public int Getsize() {
        return NodeRecordList.Count;
    }

    public NodeRecord GetSmallestElement() {
        NodeRecord TmpNodeRecord = new NodeRecord();
        TmpNodeRecord.EstimatedTotalCost = float.MaxValue;

        foreach (NodeRecord NodeRecord in NodeRecordList) {
            if (NodeRecord.EstimatedTotalCost < TmpNodeRecord.EstimatedTotalCost) {
                TmpNodeRecord = NodeRecord;
            }
        }
        return TmpNodeRecord;
    }

    public bool Contains(GameObject Node) {
        foreach (NodeRecord NodeRecod in NodeRecordList) {
            if (NodeRecod.Node.Equals(Node)) {
                return true;
            }
        }
        return false;
    }

    public NodeRecord Find(GameObject Node) {
        foreach (NodeRecord NodeRecord in NodeRecordList) {
            if (NodeRecord.Node.Equals(Node)) {
                return NodeRecord;
            }
        }
        return null;
    }

}
