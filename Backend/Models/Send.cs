namespace Backend.Models; 

public class Send {
    public long Id { get; set; }
    
    public string UserId { get; set; }
    
    public long ClimbingRouteId { get; set; }
}