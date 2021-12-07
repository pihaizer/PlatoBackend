using Backend.ViewModels;

namespace Backend.Services; 

public interface ICommentsService {
    public Task<List<CommentViewModel>> GetByRouteId(long id, int count);
}