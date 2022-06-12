using System;
using UnityEngine;


public class ACOCar : ACOAgent
{
    public float Speed = 10.0f; // max speed of the car
    public parcel Parcel = new parcel(); //parcel object
    public bool Waiting = false; // is the car waiting
    public bool Delivering = false; //is car delivering

    //car event handlers
    public event Action<ACOCar, pathinfo> NewConnection;
    public event Action<ACOCar, GameObject> PathEnd;
    public event Action<ACOCar, GameObject> ReachedGoal;

    //rotate car
    [SerializeField]
    private Vector3 Offset = Vector3.zero;

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

    //turn car
    private void LookAt(Transform lookAt)
    {
        this.transform.LookAt(lookAt.position);
        this.transform.eulerAngles = this.transform.eulerAngles - Offset;
    }
}
