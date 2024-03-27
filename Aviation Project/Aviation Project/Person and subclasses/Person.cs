using System;

namespace Aviation_Project
{
    public class Person : Object
    {
        public string Name { get; set; }
        public UInt16 Age { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public Person(string code, UInt64 id, string name, UInt16 age, string phone, string email) : base(code, id)
        {
            Name = name;
            Age = age;
            Phone = phone;
            Email = email;
        }

        public Person(string[] info) : base(info)
        {
            Name = info[2];
            Age = ushort.Parse(info[3]);
            Phone = info[4];
            Email = info[5];
        }
    }
}
