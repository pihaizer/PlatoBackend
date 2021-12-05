namespace Backend.Models;

public class ClimbingRoute {
    public long Id { get; set; }

    public int Grade { get; set; }

    public string? Color { get; set; }

    public DateTimeOffset InstallDate { get; set; }
    
    public string Setter { get; set; }
}