namespace Aviation_Project;

public abstract class Media // I think that's gonna be the visitor
{
    public string Name;
    
    public Media(string name)
    {
        Name = name;
    }
    public abstract string GetNewsReport(Airport reportable);
    public abstract string GetNewsReport(PassengerPlane reportable);
    public abstract string GetNewsReport(CargoPlane reportable);
}