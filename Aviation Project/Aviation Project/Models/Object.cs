using System.Reflection.Metadata.Ecma335;

namespace Aviation_Project
{
    public abstract class Object
    {
        public string Code { get; set; }
        public UInt64 ID  { get; set; }

        public Object(string code, UInt64 id)
        {
            Code = code;
            ID = id;
        }

        public Object(string[] info)
        {
            Code = info[0];
            ID = UInt64.Parse(info[1]);
        }
    }
}