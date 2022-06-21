using Backend.Context;
using Backend.InputModels;
using Backend.Models;
using Backend.Services;

using FirebaseAdmin.Auth;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompetitionsController : ControllerBase {
    readonly PlatoContext _context;
    readonly IStorageService _storage;

    public CompetitionsController(PlatoContext context, IStorageService storage) {
        _context = context;
        _storage = storage;
    }

    [HttpGet]
    public async Task<List<Competition>> GetAll() {
        return await _context.Competitions
            .OrderByDescending(news => news.StartTimestamp)
            .ToListAsync();
    }

    [HttpPost("{competitionId:long}/Competitors")]
    //TODO: Limit access
    //[RequireAdmin]
    public async Task<ActionResult<string>> PostCompetitors(
        [FromRoute] long competitionId,
        [FromBody] CompetitorPostInput competitorInput
    ) {
        var competitor = new Competitor();
        competitor.CompetitionId = competitionId;
        competitor.UserId = competitorInput.UserId;
        competitor.Group = competitorInput.Group;

        EntityEntry<Competitor> addedCompetitor = _context.Competitors.Add(competitor);
        await _context.SaveChangesAsync();

        return new ActionResult<string>(addedCompetitor.Entity.UserId);
    }

    [HttpGet("{competitionId:long}/Competitors/{userId}")]
    public async Task<Competitor?> GetCompetitorById(
        [FromRoute] long competitionId,
        [FromRoute] string userId
    ) {
        Competitor? competitor = await _context.Competitors.FirstOrDefaultAsync(competitor =>
            competitor.UserId == userId && competitor.CompetitionId == competitionId
        );

        //TODO: Throw 404
        //if (competitor == null) return NotFound();

        return competitor;
    }
}