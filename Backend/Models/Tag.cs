namespace Backend.Models; 

public class Tag {
    public long Id { get; set; }
    
    public string Value { get; set; }

    public ICollection<ClimbingRoute>? Routes { get; set; }
}