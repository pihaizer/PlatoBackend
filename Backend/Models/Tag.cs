using Microsoft.EntityFrameworkCore;

namespace Backend.Models; 

[Index(nameof(Value), IsUnique = true)]
public class Tag {
    public long Id { get; set; }
    
    public string Value { get; set; }

    public ICollection<ClimbingRoute>? Routes { get; set; }
}