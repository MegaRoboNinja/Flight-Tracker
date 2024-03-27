using System;

namespace Aviation_Project
{
    public class CargoPlane : Plane
    {
        public float MaxLoad { get; set; }

        public CargoPlane(string code, UInt64 id, string serial, string country, string model, float maxLoad) : base(code, id, serial, country, model)
        {
            MaxLoad = maxLoad;
        }

        public CargoPlane(string[] info) : base(info)
        {
            MaxLoad = float.Parse(info[5]);
        }
    }
}