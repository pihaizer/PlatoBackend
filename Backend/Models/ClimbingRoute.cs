using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class ClimbingRoute {
    public long Id { get; set; }

    public int Grade { get; set; }

    public string? Color { get; set; }

    public DateTimeOffset InstallDate { get; set; }
    
    public virtual Setter? Setter { get; set; }
    
    public string? PictureUrl { get; set; }

    public virtual ICollection<Tag>? Tags { get; set; }
    
    [NotMapped]
    public ICollection<long> TagIds { get; set; }

    public int Status { get; set; } = 0;
    
    public virtual ClimbingRouteModel? Model { get; set; }
    
    
    public virtual ICollection<Like>? Likes { get; set; }
    
    [NotMapped]
    public int LikesCount { get; set; }

    public virtual ICollection<Send>? Sends { get; set; }
    
    [NotMapped]
    public int SendsCount { get; set; }

    public virtual ICollection<Comment>? Comments { get; set; }
    
    [NotMapped]
    public int CommentsCount { get; set; }
}