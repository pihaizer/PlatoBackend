using Backend.Context;
using Backend.Models;
using Backend.ViewModels;

using FirebaseAdmin.Auth;

using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Impl; 

public class CommentsService : ICommentsService {
    readonly PlatoContext _context;
    
    public CommentsService(PlatoContext context) {
        _context = context;
    }
    
    public async Task<List<CommentViewModel>> GetByRouteId(long routeId, int count) {
        List<Comment> comments = await _context.Comments
            .Where(comment => comment.ClimbingRouteId == routeId)
            .OrderByDescending(comment => comment.DateTime)
            .Take(count)
            .ToListAsync();

        IEnumerable<string> userIds = comments.Select(comment => comment.UserId).Distinct();

        List<User> usersResult = await _context.Users
            .Where(user => userIds.Contains(user.FirebaseId))
            .ToListAsync();

        Dictionary<string, User> users = usersResult.ToDictionary(
            user => user.FirebaseId,
            user => user);

        var commentViewModels = new List<CommentViewModel>();

        foreach (Comment comment in comments) {
            var commentViewModel = new CommentViewModel(comment);
            
            User user = users[comment.UserId];
            commentViewModel.UserName = user.Nickname ?? "Anonymous";
            commentViewModel.UserPhotoUrl = user.PhotoUrl;
            
            commentViewModels.Add(commentViewModel);
        }

        return commentViewModels;
    }
}