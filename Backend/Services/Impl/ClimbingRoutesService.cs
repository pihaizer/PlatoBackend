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
        List<ClimbingRoute> routes = await _context.Routes
            .Include(route => route.Tags).ToListAsync();

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
                viewModel.TagIds = route.Tags.Select(tag => tag.TagId).ToList();
            }

            result.Add(viewModel);
        }

        return result;
    }

    public async Task<ActionResult<long>> CreateRoute(ClimbingRouteInput input) {
        var newRoute = input.ToClimbingRoute();

        if (input.TagIds != null) {
            newRoute.Tags = input.TagIds
                .Select(tag => new ClimbingRouteTag() { TagId = tag })
                .ToList();
        }

        if (input.PictureUrl != null) {
            newRoute.PictureUrl = input.PictureUrl;
        } else if (input.PictureBase64 != null) {
            newRoute.PictureUrl = await _storage.UploadPictureBase64Async(input.PictureBase64);
        } else {
            newRoute.PictureUrl = null;
        }

        EntityEntry<ClimbingRoute> addedRoute = _context.Routes.Add(newRoute);
        await _context.SaveChangesAsync();

        return new ActionResult<long>(addedRoute.Entity.Id);
    }

    public async Task<IActionResult> UpdateRoute(long id, ClimbingRouteInput input) {
        ClimbingRoute? oldRoute = await _context.Routes
            .Include(route => route.Tags)
            .FirstOrDefaultAsync(route => route.Id == id);
        if (oldRoute == null) return new NotFoundResult();
        oldRoute.Grade = input.Grade;
        oldRoute.Color = input.Color;
        oldRoute.Setter = input.Setter;
        oldRoute.ModelId = input.ModelId;

        if (input.TagIds != null) {
            List<ClimbingRouteTag> newTags = input.TagIds.Select(tag => new ClimbingRouteTag {
                TagId = tag,
                ClimbingRouteId = oldRoute.Id
            }).ToList();

            if (oldRoute.Tags != null) {
                _context.RouteTags.RemoveRange(oldRoute.Tags.Except(newTags));
            }

            _context.RouteTags.AddRange(
                newTags.Except(oldRoute.Tags ?? ArraySegment<ClimbingRouteTag>.Empty));
        } else {
            oldRoute.Tags = null;
        }

        if (input.PictureUrl != null) {
            oldRoute.PictureUrl = input.PictureUrl;
        } else if (input.PictureBase64 != null) {
            oldRoute.PictureUrl = await _storage.UploadPictureBase64Async(input.PictureBase64);
        } else {
            oldRoute.PictureUrl = null;
        }

        _context.Entry(oldRoute).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return new OkResult();
    }
}