using Backend.Context;
using Backend.Models;
using Backend.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase {
    readonly PlatoContext _context;

    public TagsController(PlatoContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<List<TagViewModel>> GetAll() {
        return await _context.Tags.Select(tag => new TagViewModel(tag)).ToListAsync();
    }

    [HttpPost]
    [RequireAdmin]
    public async Task<IActionResult> CreateTag(string value) {
        var tag = new Tag { Value = value };
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{tagId:long}")]
    [RequireAdmin]
    public async Task<IActionResult> DeleteTag(long tagId) {
        Tag? tag = await _context.Tags.FindAsync(tagId);
        if (tag == null) return NotFound();
        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();
        return Ok();
    }
}