using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;

namespace Aviation_Project
{
    public class ObjectFactory
    {
        private static Dictionary<string, Func<string[],Object>> _string_creators = new Dictionary<string, Func<string[], Object>>
        {
            { "C",  info => new CrewMember(info) },
            { "P",  info => new Passenger(info) },
            { "CA", info => new Cargo(info) },
            { "CP", info => new CargoPlane(info) },
            { "PP", info => new PassengerPlane(info) },
            { "AI", info => new Airport(info) },
            { "FL", info => new Flight(info) }
        };
        private static Dictionary<string, Func<BinaryReader,Object>> _byte_creators = new Dictionary<string, Func<BinaryReader, Object>>
        {
            { "NCR",  info => CrewMemberGen.Generate(info) },
            { "NPA",  info => PassengerGen.Generate(info) },
            { "NCA", info => CargoGen.Generate(info) },
            { "NCP", info => CargoPlaneGen.Generate(info) },
            { "NPP", info => PassengerPlaneGen.Generate(info) },
            { "NAI", info => AirportGen.Generate(info) },
            { "NFL", info => FlightGen.Generate(info) }
        }; // TODO: add these constructors or create factories
        
        public static Object CreateObject(string input)
        {
            string[] info = input.Split(',');
            Func<string[], Object> f = _string_creators[info[0]];
            return f(info);
        }
        
        public static Object CreateObject(byte [] input)
        {
            string code = Encoding.ASCII.GetString(input[0..3]);
            Func<BinaryReader, Object> f = _byte_creators[code];
            MemoryStream stream = new MemoryStream(input);
            BinaryReader reader = new BinaryReader(stream);
            return f(reader);
        }
    }
}