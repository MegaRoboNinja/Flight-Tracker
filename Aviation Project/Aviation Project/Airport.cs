using System;

namespace Aviation_Project
{
    public class Airport : Object
    {
        public static Dictionary<ulong, Airport> allAirports { get; set; } = new Dictionary<ulong, Airport>();
        public string Name { get; set; }
        public string AirportCode { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public float AMSL { get; set; }
        public string Country { get; set; }

        public Airport(string code, UInt64 id, string name, string airportCode, float longitude, float latitude, float amsl, string country) : base(code, id)
        {
            Code = code;
            Name = name;
            AirportCode = airportCode;
            Longitude = longitude;
            Latitude = latitude;
            AMSL = amsl;
            Country = country;
            allAirports[id] = this;
        }

        public Airport(string[] info) : base(info)
        {
            Name = info[2];
            Code = info[3];
            Longitude = float.Parse(info[4]);
            Latitude = float.Parse(info[5]);
            AMSL = float.Parse(info[6]);
            Country = info[7];
            allAirports[ulong.Parse(info[1])] = this;
        }
    }
}