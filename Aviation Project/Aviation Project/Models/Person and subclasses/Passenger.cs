using System;

namespace Aviation_Project
{
    public class Passenger : Person
    {
        public string Class { get; set; }
        public UInt64 Miles { get; set; }

        public Passenger(string code, UInt64 id, string name, UInt16 age, string phone, string email, string @class, ulong miles) : base(code, id, name, age, phone, email)
        {
            Class = @class;
            Miles = miles;
        }

        public Passenger(string[] info) : base(info)
        {
            Class = info[6];
            Miles = ushort.Parse(info[7]);
        }
    }
}