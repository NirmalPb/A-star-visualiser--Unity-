using System;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceCar : MonoBehaviour
{
    public float Speed = 10.0f; // max speed of the car
    public parcel Parcel = new parcel(); //parcel object
    public bool Waiting = false; // is the car waiting
   
    public event Action<AdvanceCar, Connection> newConnection; 
    public event Action<AdvanceCar, GameObject> pathEnd;
   
    private List<Connection> drivePath = null; // current drive path of the car
    private GameObject targetNode = null; // way point car is going to
    private int nodeIndex = 0; // current index of the way point list


    [SerializeField]
    private Vector3 ModelRotationOffset = Vector3.zero; // car init rotaion

    void Update()
    {
        if (targetNode)
        {
            LookAt(targetNode.transform);
            if (!Waiting)
            {
                // Move Car
                Move();
                float nextNodeDistance = Vector3.Distance(transform.position, targetNode.transform.position); //dis to next node
                if (nextNodeDistance < 0.001f)
                {
                   nodeIndex++;
                    if (nodeIndex > 0 && nodeIndex < drivePath.Count)
                    {
                        Connection currentTargetConnection = drivePath[nodeIndex];
                        targetNode = currentTargetConnection.ToNode;

                        newConnection?.Invoke(this, currentTargetConnection);
                    }
                }
                float finalNodeDistance = Vector3.Distance(transform.position, drivePath[drivePath.Count - 1].ToNode.transform.position);
                if (finalNodeDistance < 0.001f)
                {
                    Connection finalConnection = drivePath[drivePath.Count - 1];
                    pathEnd?.Invoke(this, finalConnection.ToNode);
                    drivePath = null;
                    targetNode = null;
                    nodeIndex = 0;
                    
                }
            }
        }
    }
    
    //drive along to the next node
    public void DriveAlong(List<Connection> connectionPath)
    {
       
        drivePath = null;
        targetNode = null;

        // error check
        if (connectionPath == null)
        {
            Debug.LogError("connectionPath invalid");
            return;
        }

        drivePath = connectionPath;
        

        if (targetNode == null)
        {
            this.transform.position = drivePath[nodeIndex].FromNode.transform.position;
            Connection nextConnection = drivePath[nodeIndex];
            targetNode = nextConnection.ToNode;
            newConnection?.Invoke(this, nextConnection);
        }
    }

    //move car to the next location
    private void Move()
    {
        float totalPackageSpeedReduction = CalculateCargoSpeedReduction();
        float currentCargoSpeed = Speed - totalPackageSpeedReduction;
        float step = currentCargoSpeed * Time.deltaTime;
        Vector3 stepVector = Vector3.MoveTowards(this.transform.position, targetNode.transform.position, step);
        this.transform.position = stepVector;
    }

   //turn car
    private void LookAt(Transform lookAtTransform)
    {
        this.transform.LookAt(lookAtTransform.position);
        this.transform.eulerAngles = this.transform.eulerAngles - ModelRotationOffset;
    }
    
    //stop the car
    public void Pause()
    {
        if (!Waiting)
            Waiting = true;
    }

    //move car if stopped
    public void Movement()
    {
        if (Waiting)
            Waiting = false;
    }

    //complete delivery
    public void Deliver()
    {
        Debug.Log($"dropping of parcel at '{targetNode.name}'");
        Parcel.DeliveryComplete(1);

    }

    // change parcel amount
    public void SetPackages(int packageAmt)
    {
        Parcel.AddPackages(packageAmt);
    }

    //return the speed reduction
    private float CalculateCargoSpeedReduction()
    {
        float reduction = Speed * 0.9f / 15 * Parcel.GetparcelCount;
        return reduction;
    }
}
