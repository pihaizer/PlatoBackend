using Backend.Context;
using Backend.InputModels;
using Backend.Models;
using Backend.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Backend.Services; 

public class ClimbingRoutesService : IClimbingRoutesService {
    readonly PlatoContext _context;
    readonly IStorageService _storage;
    
    public ClimbingRoutesService(PlatoContext context, IStorageService storageService) {
        _context = context;
        _storage = storageService;
    }

    public async Task<List<ClimbingRouteViewModel>> GetAll() {
        List<ClimbingRoute> routes = await _context.Routes.ToListAsync();

        var likesCounts = await _context.Likes
            .GroupBy(p => p.ClimbingRouteId)
            .Select(group => new { id = @group.Key, count = @group.Count() })
            .ToListAsync();

        var sendsCounts = await _context.Sends
            .GroupBy(p => p.ClimbingRouteId)
            .Select(group => new { id = @group.Key, count = @group.Count() })
            .ToListAsync();

        var commentsCounts = await _context.Comments
            .GroupBy(p => p.ClimbingRouteId)
            .Select(group => new { id = @group.Key, count = @group.Count() })
            .ToListAsync();
        
        var result = new List<ClimbingRouteViewModel>();
        
        foreach (ClimbingRoute route in routes) {
            var viewModel = new ClimbingRouteViewModel(route);
            
            viewModel.LikesCount = likesCounts
                .FirstOrDefault(arg => arg.id == route.Id)?.count ?? 0;
            
            viewModel.SendsCount = sendsCounts
                .FirstOrDefault(arg => arg.id == route.Id)?.count ?? 0;
            
            viewModel.CommentsCount = commentsCounts
                .FirstOrDefault(arg => arg.id == route.Id)?.count ?? 0;

            if (route.Tags != null) {
                viewModel.Tags = route.Tags.Select(tag => new TagViewModel(tag)).ToList();
            }
            
            result.Add(viewModel);
        }

        return result;
    }

    public async Task<ActionResult<long>> CreateRoute(ClimbingRoutePostInput input) {
        var newRoute = input.ToClimbingRoute();
        
        if (input.TagIds != null) {
            List<Tag> tags = await _context.Tags
                .Where(tag => input.TagIds.Contains(tag.Id))
                .ToListAsync();
            newRoute.Tags = tags;
        }

        if (input.PictureBase64 != null) {
            newRoute.PictureUrl = await _storage.UploadPictureBase64(input.PictureBase64);
        }

        if (input.ModelId != null) {
            ClimbingRouteModel? model = await _context.RouteModels
                .FirstOrDefaultAsync(model => model.Id == input.ModelId);

            if (model == null) {
                return new NotFoundResult();
            }

            newRoute.Model = model;
        }

        EntityEntry<ClimbingRoute> addedRoute = _context.Routes.Add(newRoute);
        await _context.SaveChangesAsync();

        return new ActionResult<long>(addedRoute.Entity.Id);
    } 
}