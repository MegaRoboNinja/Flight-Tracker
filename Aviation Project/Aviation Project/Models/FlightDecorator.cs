namespace Aviation_Project;

/// <summary>
/// Decorator for the Flight class facilitating displaying the flight in the GUI.
/// It's necessary since the plane's position can be updated mid-flight, making it impossible
/// to assess it's position given the origin and target airport position and the time.
/// </summary>
public class FlightDecorator: Flight
{   
    protected Flight _flight; // wrapped object
    
    // these will change in case of mid-flight teleportation:
    public Single OriginLongitude { get; set; }
    public Single OriginLatitude { get; set; }
    /// <summary> When the flight flew out of the Origin. If the flight wasn't teleported it's equal to the takeoff time. </summary>
    public DateTime StartTime { get; set; }
    
    public FlightDecorator(Flight flight) 
        : base(flight.Code, flight.ID, flight.OriginID, flight.TargetID, flight.TakeoffTime, flight.LandingTime, flight.Longitude, flight.Latitude, flight.AMSL, flight.PlaneID, flight.CrewIDs, flight.LoadIDs)
    {
        _flight = flight;
        
        // Get the origin airport TODO: (maybe) change this to use the DB instead of the class list
        Airport origin = Airport.allAirports[flight.OriginID];
        OriginLongitude = origin.Longitude;
        OriginLatitude = origin.Latitude;
        
        // Set the start time
        StartTime = flight.TakeoffTime;
    }
}