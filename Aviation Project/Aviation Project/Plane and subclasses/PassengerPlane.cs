using System;

namespace Aviation_Project
{
    public class PassengerPlane : Plane, IReportable
    {
        public static List<PassengerPlane> AllPassengerPlanes { get; set; } = new List<PassengerPlane>();
        public UInt16 FirstClassSize { get; set; }
        public UInt16 BusinessClassSize { get; set; }
        public UInt16 EconomyClassSize { get; set; }

        public PassengerPlane(string code, UInt64 id, string serial, string country, string model, ushort firstClassSize, ushort businessClassSize, ushort economyClassSize) : base(code, id, serial, country, model)
        {
            FirstClassSize = firstClassSize;
            BusinessClassSize = businessClassSize;
            EconomyClassSize = economyClassSize;
            
            AllPassengerPlanes.Add(this);
        }
        public PassengerPlane(string[] info) : base(info)
        {
            FirstClassSize = ushort.Parse(info[5]);
            BusinessClassSize = ushort.Parse(info[6]);
            EconomyClassSize = ushort.Parse(info[7]);
            
            AllPassengerPlanes.Add(this);
        }
        
        public string acceptReport(Media medium)
        {
            return medium.GetNewsReport(this);
        }
    }
}