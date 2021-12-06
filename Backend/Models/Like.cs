namespace Backend.Models; 

public class Like {
    public long Id { get; set; }
    
    public string UserId { get; set; }
    
    public long ClimbingRouteId { get; set; }
}