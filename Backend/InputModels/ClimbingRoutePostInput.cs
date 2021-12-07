using Backend.Models;

namespace Backend.InputModels; 

public class ClimbingRoutePostInput {
    public long InstallDateTimestamp { get; set; }

    public int Grade { get; set; }
    public string Color { get; set; } = string.Empty;
    public string? PictureBase64 { get; set; }
    
    public long? ModelId { get; set; }
    public string? Setter { get; set; }
    public List<long>? TagIds { get; set; }

    public ClimbingRoute ToClimbingRoute() {
        return new ClimbingRoute {
            Grade = Grade,
            Color = Color,
            Setter = Setter
        };
    }
}