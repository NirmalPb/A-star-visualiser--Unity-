using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SimpleCar : MonoBehaviour
{
    //max Speed
    public float MaxSpeed = 15.0f;
    public event Action OnPathEnd;

    private List<Connection> drivePath = null;
    
    [SerializeField]
    private Vector3 ModelOffset = Vector3.zero; //change this rotate the car agent

    //next node 
    private GameObject nextNode = null;

    //current index of path list
    private int nextIndex = 0;

    //des tolerance
    private const float DES_TOL = 0.001f;

    // Update is called once per frame
    #region MonoBehaviours 
    void Update()
    {
        DoMovement();

        // If nearly reached destination
        float nextNodeDistance = Vector3.Distance(transform.position, nextNode.transform.position);
        if (nextNodeDistance < DES_TOL)
        {
            // Increment and set new target game object
            nextIndex++;

            // Move target node to next node index
            if (nextIndex > 0 && nextIndex < drivePath.Count)
            {
                nextNode = drivePath[nextIndex].ToNode;
            }
            else
            {
                //Debug.LogError("TargetNodeIndex is out of bounds!");
            }
        }

        // Check if reached final node yet
        float finalNodeDistance = Vector3.Distance(transform.position, drivePath[drivePath.Count - 1].ToNode.transform.position);
        if (finalNodeDistance < DES_TOL)
        {
            // Invoke event for reached path end and remove target
            OnPathEnd?.Invoke();
            ResetPath();
        }
    
    }
    #endregion

    //drive along the con path
    public void DriveAlong(List<Connection> connectionPath)
    {
        if (connectionPath == null || connectionPath != null && connectionPath.Count <= 0)
        {
            Debug.LogError("connectionPath invalid");
            return;
        }

        drivePath = connectionPath;
        Debug.Log($"Car Has '{drivePath.Count}' connections to travel");

        if (nextNode == null)
        {
            // find the next index of the car path
            this.transform.position = drivePath[nextIndex].FromNode.transform.position;
            nextNode = drivePath[nextIndex].ToNode;
        }
    }

    //move the car
    private void DoMovement()
    {
        // Calculate distance this step and move object
        float step = MaxSpeed * Time.deltaTime;

        // Move towards destination with offset
        Vector3 stepVector = Vector3.MoveTowards(this.transform.position, nextNode.transform.position, step);
        this.transform.position = stepVector;

        // Set rotation to look at next connection
        this.transform.LookAt(nextNode.transform.position);
        transform.eulerAngles = transform.eulerAngles - ModelOffset;

    }

    //display info
    public void DeliverPackage()
    {
        Debug.Log($"Delivered to '{nextNode.name}'");
    }

    //reset path info
    private void ResetPath()
    {
        nextNode = null;
        nextIndex = 0;
        drivePath = null;
    }
}
