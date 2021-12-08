using Backend.Models;

namespace Backend.InputModels; 

public class NewsPostInput {
    public string Header { get; set; }
    
    public string Text { get; set; }
    
    public long PublishTimestamp { get; set; }
    
    public string? MainPictureUrl { get; set; }
    
    public string? MainPictureBase64 { get; set; }

    public News ToNews() {
        var news = new News {
            Header = Header,
            Text = Text,
            PublishTimestamp = PublishTimestamp,
            MainPictureUrl = MainPictureBase64
        };
        return news;
    }
}