namespace Aviation_Project;

public class Television : Media
{
    public Television(string name) : base(name) { }
    public override string GetNewsReport(Airport reportable)
    {
        return $"<An image of {reportable.Name} airport>";
    }
    
    public override string GetNewsReport(PassengerPlane reportable)
    {
        return $"<An image of {reportable.Serial} cargo plane>";
    }
    
    public override string GetNewsReport(CargoPlane reportable)
    {
        return $"<An image of\n{reportable.Serial}\npassenger plane>";
    }
}