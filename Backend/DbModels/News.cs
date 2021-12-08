namespace Backend.Models; 

public class News {
    public long Id { get; set; }
    
    public string Header { get; set; }
    
    public string Text { get; set; }
    
    public long PublishTimestamp { get; set; }
    
    public string MainPictureUrl { get; set; }
}