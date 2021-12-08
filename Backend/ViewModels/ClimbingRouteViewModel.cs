using Backend.Models;

namespace Backend.ViewModels; 

public class ClimbingRouteViewModel {
    public long Id { get; set; }

    public int Status { get; set; } = 0;

    public long InstallDateTimestamp { get; set; }

    public int Grade { get; set; }

    public string Color { get; set; } = string.Empty;
    
    public string? SetterName { get; set; }
    
    public string? PictureUrl { get; set; }
    
    public string? ModelUrl { get; set; }
    
    public ICollection<long>? TagIds { get; set; }
    
    public int LikesCount { get; set; }
    
    public int SendsCount { get; set; }
    
    public int CommentsCount { get; set; }
    
    public ClimbingRouteViewModel() {}

    public ClimbingRouteViewModel(ClimbingRoute climbingRoute) {
        Id = climbingRoute.Id;
        Status = climbingRoute.Status;
        InstallDateTimestamp = climbingRoute.InstallDate.ToUnixTimeSeconds();
        Grade = climbingRoute.Grade;
        Color = climbingRoute.Color;
        SetterName = climbingRoute.Setter;
        PictureUrl = climbingRoute.PictureUrl;
        ModelUrl = climbingRoute.Model?.Url;
    }
}