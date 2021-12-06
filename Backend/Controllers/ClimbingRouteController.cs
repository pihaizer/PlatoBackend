using Backend.Context;
using Backend.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using EntityState = Microsoft.EntityFrameworkCore.EntityState;

namespace Backend.Controllers;

[ApiController]
[Route("api/Route")]
public class ClimbingRouteController : ControllerBase {
    readonly PlatoContext _context;

    public ClimbingRouteController(PlatoContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<List<ClimbingRoute>> GetAll() {
        List<ClimbingRoute> routes = await _context.Routes.Include(route => route.Tags).ToListAsync();
        
        var likesCounts = await _context.Likes
                .GroupBy(p => p.ClimbingRouteId)
                .Select(group => new { id = @group.Key, count = @group.Count() }).ToListAsync();
        
        foreach (ClimbingRoute route in routes) {
            var likesGroup = likesCounts.FirstOrDefault(arg => arg.id == route.Id);
            route.LikesCount = likesGroup?.count ?? 0;
            if(route.Tags != null) route.TagIds = route.Tags.Select(tag => tag.Id).ToList();
            route.Tags = null;
        }

        return routes;
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ClimbingRoute>> GetById(long id) {
        ClimbingRoute? route = await _context.Routes.FindAsync(id);

        if (route == null) {
            return NotFound();
        }

        return route;
    }

    [HttpPost]
    public async Task<ActionResult<long>> Post(ClimbingRoute climbingRoute) {
        if (await _context.Routes.FindAsync(climbingRoute.Id) != null) {
            return Conflict();
        }

        List<Tag> tags = await _context.Tags
            .Where(tag => climbingRoute.TagIds.Contains(tag.Id)).ToListAsync();
        climbingRoute.Tags = tags;

        _context.Routes.Add(climbingRoute);
        await _context.SaveChangesAsync();

        return Ok();
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

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeleteById(long id) {
        ClimbingRoute? route = await _context.Routes.FindAsync(id);

        if (route == null) return BadRequest();

        _context.Routes.Remove(route);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("Count")]
    public async Task<int> GetLikesCount() {
        return await EntityFrameworkQueryableExtensions.CountAsync(_context.Likes);
    }
    
    [HttpPost("Like/{routeId:long}")]
    public async Task<IActionResult> LikeRoute(long routeId, [FromQuery] string userId) {
        _context.Likes.Add(new Like { UserId = userId, ClimbingRouteId = routeId });
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPost("Send/{routeId:long}")]
    public async Task<IActionResult> SendRoute(long routeId, [FromQuery] string userId) {
        _context.Sends.Add(new Send() { UserId = userId, ClimbingRouteId = routeId });
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPost("Comment/{routeId:long}")]
    public async Task<IActionResult> CommentRoute(long routeId, [FromQuery] string userId, [FromBody] string message) {
        _context.Comments.Add(new Comment() { UserId = userId, ClimbingRouteId = routeId, Message = message});
        await _context.SaveChangesAsync();
        return Ok();
    }

    private bool RouteExists(long id) {
        return _context.Routes.Any(e => e.Id == id);
    }
}