using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointCon : MonoBehaviour
    
    
{
    //List of all connections to the node
    [SerializeField]
    private List<GameObject> connections = new List<GameObject>();

    public List<GameObject> Connections {
        get { return connections; }
    }

    [SerializeField]
    private enum waypointPropsList { standard, Goal};
    [SerializeField]
    private waypointPropsList WaypointType = waypointPropsList.standard;

    private bool objectSelected = false;
    private Vector3 NoOffSet = new Vector3(0, 0, 0);
    private Vector3 UpOffSet = new Vector3(0, 0.1f, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Draw debug objects in the editor and during editor play
    private void OnDrawGizmos()
    {
        if (objectSelected)
        {
            DrawWaypoint(Color.red, Color.magenta, UpOffSet);
        }
        else {
            DrawWaypoint(Color.yellow, Color.blue, NoOffSet);
        }
        objectSelected = false;
    }

    //Draws debug objects when object is selected
    private void OnDrawGizmosSelected()
    {
        objectSelected = true;
    }

    private void DrawWaypoint(Color WaypointColor, Color ConnectionColor, Vector3 offSet) {
        Gizmos.color = WaypointColor;
        Gizmos.DrawSphere(transform.position, 0.2f);

        for (int i = 0; i < Connections.Count; i++) {
            if (Connections[i] != null) {
                Gizmos.color = ConnectionColor;
                Gizmos.DrawLine((transform.position + offSet), (Connections[i].transform.position + offSet));
            }
        }
    }
}
