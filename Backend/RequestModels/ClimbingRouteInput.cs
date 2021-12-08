using Backend.Models;

namespace Backend.InputModels; 

public class ClimbingRouteInput {
    public long InstallDateTimestamp { get; set; }

    public int Grade { get; set; }
    public string Color { get; set; } = string.Empty;
    public string? PictureUrl { get; set; }
    public string? PictureBase64 { get; set; }
    
    public long? ModelId { get; set; }
    public string? Setter { get; set; }
    public List<long>? TagIds { get; set; }

    public ClimbingRoute ToClimbingRoute() {
        return new ClimbingRoute {
            InstallDate = DateTimeOffset.FromUnixTimeSeconds(InstallDateTimestamp),
            Grade = Grade,
            Color = Color,
            Setter = Setter,
            ModelId = ModelId
        };
    }
}