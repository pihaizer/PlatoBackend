using Backend.Context;
using Backend.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClimbingRouteController : ControllerBase {
    readonly ClimbingRouteContext _context;

    public ClimbingRouteController(ClimbingRouteContext context) {
        _context = context;
    }
    
    [HttpGet]
    public async Task<List<ClimbingRoute>> GetAll() {
        return await _context.Routes.ToListAsync();
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
            return NotFound();
        }

        _context.Routes.Add(climbingRoute);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> Put(ClimbingRoute climbingRoute) {
        if (!RouteExists(climbingRoute.Id))
        {
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

    private bool RouteExists(long id)
    {
        return _context.Routes.Any(e => e.Id == id);
    }
}