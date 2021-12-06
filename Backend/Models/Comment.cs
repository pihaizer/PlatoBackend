namespace Backend.Models; 

public class Comment {
    public long Id { get; set; }
    
    public long ClimbingRouteId { get; set; }
    
    public string UserId { get; set; }
    
    public string Message { get; set; }
    
    public DateTimeOffset DateTime { get; set; }
}