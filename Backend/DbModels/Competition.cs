namespace Backend.Models; 

public class Competition {
    public long Id { get; set; }
    
    public string Title { get; set; }

    public string Description { get; set; }
    
    public long StartTimestamp { get; set; }

    public long EndTimestamp { get; set; }

    public string PictureUrl { get; set; }
}