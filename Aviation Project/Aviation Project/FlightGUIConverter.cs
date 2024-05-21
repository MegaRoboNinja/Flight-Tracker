using ExCSS;
using FlightTrackerGUI;
using Mapsui.Projections;

namespace Aviation_Project;

public class FlightGUIConverter : FlightGUI
{
    public FlightGUIConverter(FlightDecorator flight)
    {
        // ------------------- CALCULATE FLIGHT POSITION AND ROTATION -------------------
        WorldPosition flightPosition;
        double flightRotation;
        
        // Get only the target airport
        // Usage of the origin airport was replaced with the FlightDecorator properties which keep info
        // about where and when it started (in case of teleportation it changes)
        Airport target = Airport.allAirports[flight.TargetID];
        
        // Get the position of the flight
        TimeSpan FlightDuration = flight.LandingTime - flight.StartTime;
        TimeSpan TimeElapsed = Program.Time - flight.StartTime;
        double progress = TimeElapsed.TotalSeconds / FlightDuration.TotalSeconds;
        
        // Interpolate the position of the flight
        double latitude = flight.OriginLatitude * (1 - progress) + target.Latitude * progress;
        double longitude = flight.OriginLongitude * (1 - progress) + target.Longitude * progress;

        flightPosition = new WorldPosition(latitude, longitude);
        
        // Get the rotation of the flight
        // Calculate the difference in latitude and longitude
        (double, double) originCoords = SphericalMercator.FromLonLat(flight.OriginLongitude, flight.OriginLatitude);
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