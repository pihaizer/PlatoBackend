using Backend.Models;

namespace Backend.ViewModels; 

public class CommentViewModel {
    public long Id { get; set; }
    
    public long ClimbingRouteId { get; set; }
    
    public string Message { get; set; }
    
    public string UserId { get; set; }
    public string UserName { get; set; }
    
    public string? UserPhotoUrl { get; set; }
    
    public long Timestamp { get; set; }

    public CommentViewModel(Comment comment) {
        Id = comment.Id;
        ClimbingRouteId = comment.ClimbingRouteId;
        UserId = comment.UserId;
        Message = comment.Message;
        Timestamp = comment.DateTime.ToUnixTimeSeconds();
    }
}