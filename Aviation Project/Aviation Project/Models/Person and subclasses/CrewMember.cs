using System;

namespace Aviation_Project
{
    public class CrewMember : Person
    {
        public UInt16 Practice { get; set; }
        public string Role { get; set; }

        public CrewMember(string code, UInt64 id, string name, UInt16 age, string phone, string email, ushort practice, string role) : base(code, id, name, age, phone, email)
        {
            Practice = practice;
            Role = role;
        }

        public CrewMember(string[] info) : base(info)
        {
            Practice = ushort.Parse(info[6]);
            Role = info[7];
        }
    }
}