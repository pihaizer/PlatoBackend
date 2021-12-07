using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class ClimbingRoute {
    public long Id { get; set; }
    
    public int Status { get; set; } = 0;

    public DateTimeOffset InstallDate { get; set; }

    public int Grade { get; set; }

    public string Color { get; set; } = string.Empty;
    
    public string? PictureUrl { get; set; }
    
    public string? Setter { get; set; }
    
    public virtual ClimbingRouteModel? Model { get; set; }
    public virtual ICollection<Tag>? Tags { get; set; }
    
    public virtual ICollection<Like>? Likes { get; set; }
    
    public virtual ICollection<Send>? Sends { get; set; }
    
    public virtual ICollection<Comment>? Comments { get; set; }
    
    public virtual ICollection<ClimbingRouteBookmark>? Bookmarks { get; set; }
}