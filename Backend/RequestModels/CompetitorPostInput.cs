using Backend.Models;

namespace Backend.InputModels; 

public class CompetitorPostInput {
    public string UserId { get; set; }

    public int Group { get; set; }
}