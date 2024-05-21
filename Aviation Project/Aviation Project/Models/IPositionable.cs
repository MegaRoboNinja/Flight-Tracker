namespace Aviation_Project;
// 
public interface IPositionable
{
    public Single Longitude { get; set; }
    public Single Latitude { get; set; }
    public Single AMSL { get; set; }
}