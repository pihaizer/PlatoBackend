using Backend.Context;
using Backend.InputModels;
using Backend.Models;
using Backend.Services;
using Backend.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using EntityState = Microsoft.EntityFrameworkCore.EntityState;

namespace Backend.Controllers;

[ApiController]
[Route("api/Route")]
public class ClimbingRouteController : ControllerBase {
    readonly PlatoContext _context;
    readonly IClimbingRoutesService _routesService;
    readonly ICommentsService _commentsService;

    public ClimbingRouteController(PlatoContext context,
        IClimbingRoutesService routesService,
        ICommentsService commentsService) {
        _context = context;
        _routesService = routesService;
        _commentsService = commentsService;
    }

    [HttpGet]
    public async Task<List<ClimbingRouteViewModel>> GetAll() {
        return await _routesService.GetAll();
    }

    [HttpPost]
    public async Task<ActionResult<long>> Post(ClimbingRoutePostInput climbingRouteInput) {
        return await _routesService.CreateRoute(climbingRouteInput);
    }

    [HttpPut]
    public async Task<ActionResult> Put(ClimbingRoute climbingRoute) {
        if (!RouteExists(climbingRoute.Id)) {
            return NotFound();
        }

        _context.Entry(climbingRoute).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{routeId:long}")]
    public async Task<ActionResult> DeleteById(long routeId) {
        ClimbingRoute? route = await _context.Routes.FindAsync(routeId);

        if (route == null) return BadRequest();

        _context.Routes.Remove(route);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("{routeId:long}/Like")]
    public async Task<IActionResult> LikeRoute(long routeId, [FromQuery] string userId) {
        Like? like = await _context.Likes.FirstOrDefaultAsync(like =>
            like.UserId == userId && like.ClimbingRouteId == routeId);

        if (like != null) {
            return Conflict();
        }

        _context.Likes.Add(new Like { UserId = userId, ClimbingRouteId = routeId });
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{routeId:long}/Like")]
    public async Task<IActionResult> RemoveLikeRoute(long routeId, [FromQuery] string userId) {
        Like? like = await _context.Likes.FirstOrDefaultAsync(like =>
            like.UserId == userId && like.ClimbingRouteId == routeId);

        if (like == null) {
            return NotFound();
        }

        _context.Likes.Remove(like);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("{routeId:long}/Send")]
    public async Task<IActionResult> SendRoute(long routeId, [FromQuery] string userId) {
        Send? send = await _context.Sends.FirstOrDefaultAsync(like =>
            like.UserId == userId && like.ClimbingRouteId == routeId);

        if (send != null) {
            return Conflict();
        }

        _context.Sends.Add(new Send { UserId = userId, ClimbingRouteId = routeId });
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{routeId:long}/Send")]
    public async Task<IActionResult> RemoveSendRoute(long routeId, [FromQuery] string userId) {
        Send? send = await _context.Sends.FirstOrDefaultAsync(send =>
            send.UserId == userId && send.ClimbingRouteId == routeId);

        if (send == null) {
            return NotFound();
        }

        _context.Sends.Remove(send);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("{routeId:long}/Bookmark")]
    public async Task<IActionResult> BookmarkRoute(long routeId, [FromQuery] string userId) {
        ClimbingRouteBookmark? bookmark = await _context.Bookmarks.FirstOrDefaultAsync(bookmark =>
            bookmark.UserId == userId && bookmark.ClimbingRouteId == routeId);

        if (bookmark != null) {
            return Conflict();
        }

        _context.Bookmarks.Add(new ClimbingRouteBookmark { UserId = userId, ClimbingRouteId = routeId });
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{routeId:long}/Bookmark")]
    public async Task<IActionResult> RemoveBookmarkRoute(long routeId, [FromQuery] string userId) {
        ClimbingRouteBookmark? bookmark = await _context.Bookmarks.FirstOrDefaultAsync(bookmark =>
            bookmark.UserId == userId && bookmark.ClimbingRouteId == routeId);

        if (bookmark == null) {
            return NotFound();
        }

        _context.Bookmarks.Remove(bookmark);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("{routeId:long}/Comments")]
    public async Task<List<CommentViewModel>> GetCommentsByRouteId(long routeId,
        [FromQuery] int count) {
        return await _commentsService.GetByRouteId(routeId, count);
    }

    [HttpPost("{routeId:long}/Comment")]
    public async Task<IActionResult> CommentRoute(long routeId,
        [FromQuery] string userId, [FromBody] string message) {
        _context.Comments.Add(new Comment {
            UserId = userId,
            ClimbingRouteId = routeId,
            Message = message,
            DateTime = DateTimeOffset.UtcNow
        });
        
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{routeId:long}/Comment")]
    public async Task<IActionResult> DeleteCommentById(long routeId, [FromQuery] long commentId) {
        Comment? comment = await _context.Comments.FindAsync(commentId);
        
        if (comment == null) {
            return NotFound();
        }
        
        _context.Comments.Remove(comment);
        
        await _context.SaveChangesAsync();
        return Ok();
    }

    bool RouteExists(long id) {
        return _context.Routes.Any(e => e.Id == id);
    }
}