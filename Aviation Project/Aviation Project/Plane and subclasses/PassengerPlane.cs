using System;

namespace Aviation_Project
{
    public class PassengerPlane : Plane
    {
        public UInt16 FirstClassSize { get; set; }
        public UInt16 BusinessClassSize { get; set; }
        public UInt16 EconomyClassSize { get; set; }

        public PassengerPlane(string code, UInt64 id, string serial, string country, string model, ushort firstClassSize, ushort businessClassSize, ushort economyClassSize) : base(code, id, serial, country, model)
        {
            FirstClassSize = firstClassSize;
            BusinessClassSize = businessClassSize;
            EconomyClassSize = economyClassSize;
        }
        public PassengerPlane(string[] info) : base(info)
        {
            FirstClassSize = ushort.Parse(info[5]);
            BusinessClassSize = ushort.Parse(info[6]);
            EconomyClassSize = ushort.Parse(info[7]);
        }
    }
}