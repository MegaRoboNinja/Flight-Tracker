namespace Aviation_Project;

public class Radio : Media
{
    public Radio(string name) : base(name) { }
    public override string GetNewsReport(Airport reportable)
    {
        return $"Reporting for {this.Name},\nLadies and gentelmen, we are at the {reportable.Name} airport.";
    }
    
    public override string GetNewsReport(PassengerPlane reportable)
    {
        return $"Reporting for {this.Name}, Ladies and gentelmen, we are seeing the {reportable.Serial} aircraft fly above us.";
    }
    
    public override string GetNewsReport(CargoPlane reportable)
    {
        return $"Reporting for {this.Name}, Ladies and gentelmen, we’ve just witnessed {reportable.Serial } takeoff.";
    }
}