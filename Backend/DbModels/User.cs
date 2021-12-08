using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;

namespace Backend.Models;

[Microsoft.EntityFrameworkCore.Index(nameof(FirebaseId), IsUnique = true)]
public class User {
    [JsonIgnore]
    public long Id { get; set; }
    
    public string FirebaseId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
    
    public string? Nickname { get; set; }
    
    public string? PhotoUrl { get; set; }

    public byte Sex { get; set; }

    public long? StartDateTimestamp { get; set; }

    [NotMapped]
    public virtual ICollection<long>? LikedRouteIds { get; set; }

    [NotMapped]
    public virtual ICollection<long>? SentRouteIds { get; set; }

    [NotMapped]
    public virtual ICollection<long>? BookmarkedRouteIds { get; set; }
}