namespace Aviation_Project;

public class Newspaper : Media
{
    public Newspaper(string name) : base(name) { }
    public override string GetNewsReport(Airport reportable)
    {
        return $"{this.Name} - A report from the {reportable.Name} airport, {reportable.Country}.";
    }
    
    public override string GetNewsReport(PassengerPlane reportable)
    {
        return $"{this.Name} -An interview with the crew of {reportable.Serial}.";
    }
    
    public override string GetNewsReport(CargoPlane reportable)
    {
        return $"{this.Name} - Breaking news! {reportable.Model} aircraft loses EASA fails certification after inspection of {reportable.Serial} .";
    }
}