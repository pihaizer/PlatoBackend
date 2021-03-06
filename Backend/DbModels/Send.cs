using Microsoft.EntityFrameworkCore;

namespace Backend.Models; 

[Index(nameof(UserId), nameof(ClimbingRouteId), IsUnique = true)]
public class Send {
    public long Id { get; set; }
    
    public string UserId { get; set; }
    
    public long ClimbingRouteId { get; set; }
}