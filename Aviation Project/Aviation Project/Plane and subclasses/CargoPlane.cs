using System;

namespace Aviation_Project
{
    public class CargoPlane : Plane, IReportable
    {
        public static List<CargoPlane> allCargoPlanes { get; set; } = new List<CargoPlane>();
        public float MaxLoad { get; set; }

        public CargoPlane(string code, UInt64 id, string serial, string country, string model, float maxLoad) : base(code, id, serial, country, model)
        {
            MaxLoad = maxLoad;
            allCargoPlanes.Add(this);
        }

        public CargoPlane(string[] info) : base(info)
        {
            MaxLoad = float.Parse(info[5]);
            allCargoPlanes.Add(this);
        }
        
        public string acceptReport(Media medium)
        {
            return medium.GetNewsReport(this);
        }
    }
}