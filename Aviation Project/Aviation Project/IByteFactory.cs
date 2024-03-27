using System.CodeDom.Compiler;
using System.Runtime.InteropServices.JavaScript;
using System.Text;

namespace Aviation_Project;

public interface IByteFactory
{
    static Object Generate(BinaryReader info)
    {
        return null;
    }
}

class AirportGen : IByteFactory
{
    public static Object Generate(BinaryReader info)
    { // info still contains the code at the beginning
        info.ReadChars(7); // we already know the code and following length does not matter
        UInt64 ID = info.ReadUInt64(); 
        UInt16 nameLength = info.ReadUInt16(); // NOT A FIELD
        char[] nameChars = info.ReadChars(nameLength);
        string Name = new string(nameChars);
        char[] codeChars = info.ReadChars(3);
        string Code = new string(codeChars);
        float Longtd = info.ReadSingle();
        float Latitd = info.ReadSingle();
        float AMSL = info.ReadSingle();
        char[] countryChars = info.ReadChars(3);
        string Country = new string(countryChars);
        
        return new Airport("AI", ID, Name, Code, Longtd, Latitd, AMSL, Country);
    }
}

class CargoGen : IByteFactory
{
    public static Object Generate(BinaryReader info)
    { // info still contains the code at the beginning
        info.ReadChars(7); // we already know the code and following length does not matter
        UInt64 ID = info.ReadUInt64(); 
        float Weight = info.ReadSingle();
        char[] codeChars = info.ReadChars(3);
        string Code = new string(codeChars);
        UInt16 descLength = info.ReadUInt16(); // NOT A FIELD
        char[] descChars = info.ReadChars(descLength);
        string Description = new string(descChars);
        Description = Description.Trim('\0');
        
        return new Cargo("CA",ID, Weight, Code, Description);
    }
}

class FlightGen : IByteFactory
{
    public static Object Generate(BinaryReader info)
    {
        // info still contains the code at the beginning
        info.ReadChars(7); // we already know the code and following length does not matter
        UInt64 ID = info.ReadUInt64(); 
        UInt64 OriginID = info.ReadUInt64();
        UInt64 DestinationID = info.ReadUInt64();
        UInt64 TakeoffAfterEpoch= info.ReadUInt64();
        string TakeoffTime = DateTime.UnixEpoch.AddMilliseconds(TakeoffAfterEpoch).ToString();
        UInt64 LandingAfterEpoch= info.ReadUInt64();
        string LandingTime = DateTime.UnixEpoch.AddMilliseconds(LandingAfterEpoch).ToString();
        float Longitude = info.ReadSingle();
        float Latitude = info.ReadSingle();
        float AMSL = info.ReadSingle();
        UInt64 PlaneID = info.ReadUInt64();
        UInt16 CrewCount = info.ReadUInt16();
        UInt64[] CrewIDs = new UInt64[CrewCount];
        for (int i = 0; i < CrewCount; i++)
            CrewIDs[i] = info.ReadUInt64();
        UInt16 LoadCount = info.ReadUInt16();
        UInt64[] LoadIDs = new UInt64[LoadCount];
        for (int i = 0; i < LoadCount; i++)
            LoadIDs[i] = info.ReadUInt64();
        
        return new Flight("FL", ID, OriginID, DestinationID, TakeoffTime, LandingTime, Longitude, Latitude, AMSL, PlaneID, CrewIDs, LoadIDs);
    }
}

class PassengerGen : IByteFactory
{
    public static Object Generate(BinaryReader info)
    {
        info.ReadChars(7); // we already know the code and following length does not matter
        UInt64 ID = info.ReadUInt64(); 
        UInt16 nameLength = info.ReadUInt16(); // NOT A FIELD
        char[] nameChars = info.ReadChars(nameLength);
        string Name = new string(nameChars);
        UInt16 Age = info.ReadUInt16();
        UInt16 phoneLength = 12; //info.ReadUInt16(); // NOT A FIELD
        char[] phoneChars = info.ReadChars(phoneLength);
        string Phone = new string(phoneChars);
        UInt16 emailLength = info.ReadUInt16(); // NOT A FIELD
        char[] emailChars = info.ReadChars(emailLength);
        string Email = new string(emailChars);
        char[] classChars = info.ReadChars(1);
        string Class = new string(classChars);
        UInt64 Miles = info.ReadUInt64();
        
        return new Passenger("PA", ID, Name, Age, Phone, Email, Class, Miles);
    }
}

class CrewMemberGen : IByteFactory
{
    public static Object Generate(BinaryReader info)
    {
        info.ReadChars(7); // we already know the code and following length does not matter
        UInt64 ID = info.ReadUInt64(); 
        UInt16 nameLength = info.ReadUInt16();
        char[] nameChars = info.ReadChars(nameLength);
        string Name = new string(nameChars);
        UInt16 Age = info.ReadUInt16();
        char[] phoneChars = info.ReadChars(12);
        string Phone = new string(phoneChars);
        UInt16 emailLength = info.ReadUInt16();
        char[] emailChars = info.ReadChars(emailLength);
        string Email = new string(emailChars);
        UInt16 Practice = info.ReadUInt16();
        char[] roleChars = info.ReadChars(3);
        string Role = new string(roleChars);

        return new CrewMember("CR", ID, Name, Age, Phone, Email, Practice, Role);
    }
}

class PassengerPlaneGen : IByteFactory
{
    public static Object Generate(BinaryReader info)
    {
        info.ReadChars(7); // we already know the code and following length does not matter
        UInt64 ID = info.ReadUInt64(); 
        char[] serialChars = info.ReadChars(10);
        string Serial = new string(serialChars);
        Serial = Serial.Trim('\0');
        char[] countryChars = info.ReadChars(3);
        string Country = new string(countryChars);
        UInt16 modelLength = info.ReadUInt16();
        char[] modelChars = info.ReadChars(modelLength);
        string Model = new string(modelChars);
        UInt16 FirstClassCapacity = info.ReadUInt16();
        UInt16 BusinessCapacity = info.ReadUInt16();
        UInt16 EconomyCapacity = info.ReadUInt16();

        return new PassengerPlane("PP", ID, Serial, Country, Model, FirstClassCapacity, BusinessCapacity, EconomyCapacity);
    }
}

class CargoPlaneGen : IByteFactory
{
    public static Object Generate(BinaryReader info)
    {
        info.ReadChars(7); // we already know the code and following length does not matter
        UInt64 ID = info.ReadUInt64();
        char[] serialChars = info.ReadChars(10);
        string Serial = new string(serialChars);
        Serial = Serial.Trim('\0');
        char[] countryChars = info.ReadChars(3);
        string Country = new string(countryChars);
        UInt16 modelLength = info.ReadUInt16();
        char[] modelChars = info.ReadChars(modelLength);
        string Model = new string(modelChars);
        float CargoCapacity = info.ReadSingle();

        return new CargoPlane("CP", ID, Serial, Country, Model, CargoCapacity);
    }
}