using System.ComponentModel.DataAnnotations;

namespace Backend.Models; 

public class Competitor {
    [Key]
    public string UserId { get; set; }
    [Key]
    public long CompetitionId { get; set; }

    public int Group { get; set; }
}