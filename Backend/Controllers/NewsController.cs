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
public class NewsController : ControllerBase {
    readonly PlatoContext _context;
    readonly IStorageService _storage;

    public NewsController(PlatoContext context, IStorageService storage) {
        _context = context;
        _storage = storage;
    }

    [HttpGet("Published")]
    public async Task<List<News>> GetAllPublished([FromQuery] int count = 20) {
        var currentTimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        return await _context.News
            .Where(news => news.PublishTimestamp <= currentTimestamp)
            .OrderByDescending(news => news.PublishTimestamp)
            .Take(count)
            .ToListAsync();
    }

    [HttpGet]
    [RequireAdmin]
    public async Task<List<News>> GetAll([FromQuery] int count = 20) {
        return await _context.News
            .OrderByDescending(news => news.PublishTimestamp)
            .Take(count)
            .ToListAsync();
    }

    [HttpPost]
    [RequireAdmin]
    public async Task<ActionResult<long>> Post(NewsPostInput newsInput) {
        var news = newsInput.ToNews();

        if (newsInput.MainPictureUrl == null && newsInput.MainPictureBase64 == null) {
            return BadRequest();
        }

        if (newsInput.MainPictureUrl == null && newsInput.MainPictureBase64 != null) {
            news.MainPictureUrl =
                await _storage.UploadPictureBase64Async(newsInput.MainPictureBase64);
        }

        EntityEntry<News> newsEntity = _context.News.Add(news);
        await _context.SaveChangesAsync();
        return Ok(newsEntity.Entity.Id);
    }

    [HttpPut("{newsId:long}")]
    [RequireAdmin]
    public async Task<ActionResult> Put(NewsPostInput newsInput, long newsId) {
        News? news = await _context.News.FindAsync(newsId);
        if (news == null) return NotFound();

        news.Header = newsInput.Header;
        news.Text = newsInput.Text;
        news.PublishTimestamp = newsInput.PublishTimestamp;

        if (newsInput.MainPictureUrl == null && newsInput.MainPictureBase64 == null) {
            return BadRequest();
        } else if (newsInput.MainPictureUrl == null && newsInput.MainPictureBase64 != null) {
            news.MainPictureUrl =
                await _storage.UploadPictureBase64Async(newsInput.MainPictureBase64);
        } else if (newsInput.MainPictureUrl != null) {
            news.MainPictureUrl = newsInput.MainPictureUrl;
        }

        _context.Entry(news).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{newsId:long}")]
    [RequireAdmin]
    public async Task<ActionResult> DeleteById(long newsId) {
        News? news = await _context.News.FindAsync(newsId);
        if (news == null) return NotFound();
        _context.News.Remove(news);
        await _context.SaveChangesAsync();
        return Ok();
    }
}