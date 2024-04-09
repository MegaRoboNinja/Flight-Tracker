using ExCSS;
using FlightTrackerGUI;
using Mapsui.Projections;

namespace Aviation_Project;

public class FlightGUIAdapter : FlightGUI
{
    public FlightGUIAdapter(Flight flight)
    {
        // ------------------- CALCULATE FLIGHT POSITION AND ROTATION -------------------
        WorldPosition flightPosition;
        double flightRotation;
        
        // Get the origin and target airports
        Airport origin = Airport.allAirports[flight.OriginID];
        Airport target = Airport.allAirports[flight.TargetID];
        
        // Get the position of the flight
        TimeSpan FlightDuration = flight.LandingTime - flight.TakeoffTime;
        TimeSpan TimeElapsed = Program.Time - flight.TakeoffTime;
        double progress = TimeElapsed.TotalSeconds / FlightDuration.TotalSeconds;
        
        // Interpolate the position of the flight
        double latitude = origin.Latitude * (1 - progress) + target.Latitude * progress;
        double longitude = origin.Longitude * (1 - progress) + target.Longitude * progress;

        flightPosition = new WorldPosition(latitude, longitude);
        
        // Get the rotation of the flight
        // Calculate the difference in latitude and longitude
        (double, double) originCoords = SphericalMercator.FromLonLat(origin.Longitude, origin.Latitude);
        (double, double) targetCoords = SphericalMercator.FromLonLat(target.Longitude, target.Latitude);
        double dy = targetCoords.Item1 - originCoords.Item1;
        double dx = targetCoords.Item2 - originCoords.Item2;
        
        // Calculate the angle in radians
        flightRotation = Math.Atan2(dy, dx);
        
        // -------------------------- SET PROPERTIES --------------------------
        this.ID = flight.ID;
        this.WorldPosition = flightPosition;
        this.MapCoordRotation = flightRotation;
    }
}