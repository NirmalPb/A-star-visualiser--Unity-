public class parcel
{
    public static int MAX_PARCELS = 15;
    private int parcelCount = 0;

    //getter
    public int GetparcelCount
    {
        get { return parcelCount; }
    }

    //add parcels
    public void AddPackages(int amount)
    {
        if (parcelCount + amount > MAX_PARCELS)
        {
            parcelCount = MAX_PARCELS;
        }
        else
        {
            parcelCount += amount;
        }
    }

    //parcel delivery complete 
    public void DeliveryComplete(int amount)
    {
        if (parcelCount - amount >= 0)
        {
            parcelCount -= amount;
        }
        else
        {
            parcelCount = 0;
        }
    }

}
