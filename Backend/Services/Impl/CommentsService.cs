using Backend.Context;
using Backend.Models;
using Backend.ViewModels;

using Microsoft.EntityFrameworkCore;

namespace Backend.Services; 

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

        return comments
            .Select(comment => new CommentViewModel(comment) {
                UserName = "MOCK_USERNAME"
            })
            .ToList();
    }
}