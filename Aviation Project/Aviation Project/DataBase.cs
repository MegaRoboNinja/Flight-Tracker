using System.Runtime.InteropServices.JavaScript;
using NetworkSourceSimulator;

namespace Aviation_Project;

public class DataBase
{
    // List of all objects
    public Dictionary<UInt64, Object> allObjects { get; set; }
    public object lockAllObj { get; set; }
    
    public DataBase()
    {
        allObjects = new();
        lockAllObj = new();
    }
    public DataBase(List<Object> LoadedObjects)
    {
        lockAllObj = new();
        allObjects = new Dictionary<UInt64, Object>();
        foreach (var obj in LoadedObjects)
            this.AddObject(obj);
    }
    
    public void AddObject(Object obj)
    {
        lock (lockAllObj)
        {
            if (obj is Flight)
            {  // we want to keep all flights as flight decorators
               allObjects.Add(obj.ID, new FlightDecorator(obj as Flight));
            } else
            allObjects.Add(obj.ID, obj);
        }
    }
    
    public void Serialize()
    {
        lock (lockAllObj)
        {
            Serializer.Snapshot(allObjects.Values.ToList());
        }
    }
    
    public void IDUpdate(object sender, IDUpdateArgs args)
    {
        lock (lockAllObj)
        {
            if (!allObjects.ContainsKey(args.ObjectID))
            {
                Logger.Log($"Error: Trying to modify ID of object of ID {args.ObjectID} that cannot be found");
                return;
            }
            if (allObjects.ContainsKey(args.NewObjectID))
            {
                Logger.Log($"Error: Trying to change the ID of object form {args.ObjectID} to {args.NewObjectID} but the latter already exists in the database");
                return;
            }
        
            Logger.Log($"Object ID changed {args.ObjectID} -> {args.NewObjectID}");
            allObjects[args.ObjectID].ID = args.NewObjectID;
        }
    }
        
    public void PositionUpdate(object sender, PositionUpdateArgs args)
    {
        lock (lockAllObj)
        {
            if (!allObjects.ContainsKey(args.ObjectID))
            {
                Logger.Log($"Error: Trying to change the position of object {args.ObjectID} that cannot be found in the database");
                return;
            }
            if(!(allObjects[args.ObjectID] is FlightDecorator))
            {
                Logger.Log($"Error: Trying to change the position of object {args.ObjectID} which isn't a flight");
                return;
            }
            FlightDecorator toBeModified = allObjects[args.ObjectID] as FlightDecorator;
            Logger.Log($"Position of object {args.ObjectID} changed from {toBeModified.Longitude}:{toBeModified.Latitude} to {args.Longitude}:{args.Latitude}");
            // update the position
            toBeModified.Longitude = args.Longitude;
            toBeModified.Latitude = args.Latitude;
            toBeModified.AMSL = args.AMSL;
            
            // update the origin info (we have a new origin)
            toBeModified.OriginLongitude = args.Longitude;
            toBeModified.OriginLatitude = args.Latitude;
            toBeModified.StartTime = DateTime.Now;
        }
    }
        
    public void ContactInfoUpdate(object sender, ContactInfoUpdateArgs args)
    {
        lock (lockAllObj)
        {
            if (!allObjects.ContainsKey(args.ObjectID))
            {
                Logger.Log($"Error: Trying to modify the object {args.ObjectID} that cannot be found in the database");
                return;
            }

            if (!(allObjects[args.ObjectID] is Person))
            {
                Logger.Log($"Error: Trying to modify the contact info of the object {args.ObjectID} that isn't a Person");
                return;
            }
            Person toBeModified = allObjects[args.ObjectID] as Person;
            toBeModified.Phone = args.PhoneNumber;
            toBeModified.Email = args.EmailAddress;
        }
    }

    public List<FlightDecorator> GetAllFlights()
    {
        List<FlightDecorator> allFlights = new();
        foreach (var record in allObjects)
        {
            if (record.Value is FlightDecorator) // we keep all flights as FlightDecorators
                allFlights.Add(record.Value as FlightDecorator);
        }

        return allFlights;
    }
}