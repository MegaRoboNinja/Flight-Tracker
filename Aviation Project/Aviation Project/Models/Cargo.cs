using System;

namespace Aviation_Project
{
    public class Cargo : Object
    {
        public float Weight { get; set; }
        public string CargoCode { get; set; }
        public string Description { get; set; }

        public Cargo(string code, UInt64 id, float weight, string cargoCode, string description) : base(code, id)
        {
            Weight = weight;
            CargoCode = cargoCode;
            Description = description;
        }

        public Cargo(string[] info) : base(info)
        {
            Weight = float.Parse(info[2]);
            Code = info[3];
            Description = info[4];
        }
    }
}