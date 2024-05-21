using System;

namespace Aviation_Project
{
    public class Flight : Object, IPositionable
    {
        // public property allowing to access all flights
        public static List<Flight> allFlights { get; set; } = new List<Flight>(); // can be safely removed
        public UInt64 OriginID { get; set; }
        public UInt64 TargetID { get; set; }
        public DateTime TakeoffTime { get; set; }
        public DateTime LandingTime { get; set; }
        public Single Longitude { get; set; }
        public Single Latitude { get; set; }
        public Single AMSL { get; set; }
        public UInt64 PlaneID { get; set; }
        public UInt64[] CrewIDs { get; set; }
        public UInt64[] LoadIDs { get; set; } // cargo or passengers

        public Flight(string code, UInt64 id, ulong originId,  ulong targetId, DateTime takeoffTime, DateTime landingTime, float longitude, float latitude,  float amsl, ulong planeId, ulong[] crewIDs, ulong[] loadIDs) : base(code, id)
        {
            OriginID = originId;
            TargetID = targetId;
            TakeoffTime = takeoffTime;
            LandingTime = landingTime;
            Longitude = longitude;
            Latitude = latitude;
            AMSL = amsl;
            PlaneID = planeId;
            CrewIDs = crewIDs;
            LoadIDs = loadIDs;
            allFlights.Add(this);
        }
        
        public Flight(string[] info) : base(info)
        {
            OriginID = ushort.Parse(info[2]);
            TargetID = ushort.Parse(info[3]);
            TakeoffTime = DateTime.Parse(info[4]);
            LandingTime = DateTime.Parse(info[5]);
            Longitude = Single.Parse(info[6]);
            Latitude = Single.Parse(info[7]);
            AMSL = Single.Parse(info[8]);
            PlaneID = ushort.Parse(info[9]);
            // teraz wczytujemy tablicę zapisaną jako [ ; ; ; ]
            info[10] = info[10].Remove(0, 1);
            info[10] = info[10].Remove(info[10].Length - 1, 1);
            string[] Arr = info[10].Split(';');
            CrewIDs = new UInt64[Arr.Length];
            for (int i = 0; i < CrewIDs.Length; i++)
                CrewIDs[i] = ushort.Parse(Arr[i]);
            
            info[11] = info[11].Remove(0, 1);
            info[11] = info[11].Remove(info[11].Length - 1, 1);
            Arr = info[11].Split(';');
            LoadIDs = new UInt64[Arr.Length];
            for (int i = 0; i < LoadIDs.Length; i++)
                LoadIDs[i] = ushort.Parse(Arr[i]);
            allFlights.Add(this);
        }
    }
}