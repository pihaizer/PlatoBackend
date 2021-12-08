using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models; 

public class ClimbingRouteTag {
    [Key]
    public long ClimbingRouteId { get; set; }
    public ClimbingRoute ClimbingRoute { get; set; }
    
    [Key]
    public long TagId { get; set; }
    public Tag Tag { get; set; }
}