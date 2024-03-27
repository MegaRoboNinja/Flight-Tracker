namespace Aviation_Project
{
    public abstract class Plane : Object
    {
        public string Serial { get; set; }
        public string Country { get; set; }
        public string Model { get; set; }

        public Plane(string code, UInt64 id, string serial, string country, string model) : base(code, id)
        {
            Serial = serial;
            Country = country;
            Model = model;
        }
        public Plane(string[] info) : base(info)
        {
            Serial = info[2];
            Country = info[3];
            Model = info[4];
        }
    }
}

