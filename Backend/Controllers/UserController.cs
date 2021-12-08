using System.Security.Claims;

using Backend.Context;
using Backend.Extensions;
using Backend.Filters;
using Backend.Models;
using Backend.Services;

using FirebaseAdmin.Auth;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase {
    readonly PlatoContext _context;
    readonly IStorageService _storageService;

    public UserController(PlatoContext context, IStorageService storageService) {
        _context = context;
        _storageService = storageService;
    }

    [HttpGet("{firebaseId}")]
    public async Task<ActionResult<User>> GetUserData(string firebaseId, [FromQuery] bool fetchRoutes = false) {
        User? user = await _context.Users.FirstOrDefaultAsync(user => user.FirebaseId == firebaseId);
        if (user == null) return NotFound();

        if (fetchRoutes) {
            await FetchRoutes(user);
        }
        
        return user;
    }

    [HttpPost("Register")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Register([FromBody] User userInput) {
        string userId = User.GetFirebaseId();
        UserRecord? firebaseUser = await FirebaseAuth.DefaultInstance.GetUserAsync(userId);
        
        if (firebaseUser == null) {
            return NotFound();
        }

        if (userInput.PhotoUrl == null && userInput.PhotoBase64 != null) {
            string url = await _storageService.UploadPictureBase64Async(userInput.PhotoBase64);
            userInput.PhotoUrl = url;
        }

        _context.Users.Add(userInput);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UpdateUserInfo([FromBody] User userInput) {
        string firebaseId = User.GetFirebaseId();
        if (firebaseId != userInput.FirebaseId) return BadRequest();
        long userId = await _context.Users.Where(user => user.FirebaseId == firebaseId)
            .Select(user => user.Id).FirstOrDefaultAsync();
        userInput.Id = userId;

        if (userInput.PhotoUrl == null && userInput.PhotoBase64 != null) {
            string url = await _storageService.UploadPictureBase64Async(userInput.PhotoBase64);
            userInput.PhotoUrl = url;
        }
        
        _context.Users.Update(userInput);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("Photo")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<string>> UpdateUserPhoto([FromBody] string photoBase64) {
        User? user = await _context.Users.FindAsync(User.GetFirebaseId());

        if (user == null) return NotFound();

        string photoUrl = await _storageService.UploadPictureBase64Async(photoBase64);
        user.PhotoUrl = photoUrl;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return photoUrl;
    }

    async Task FetchRoutes(User user) {
        List<long> likedRoutes = await _context.Likes
            .Where(like => like.UserId == user.FirebaseId)
            .Select(like => like.ClimbingRouteId)
            .ToListAsync();

        List<long> sentRoutes = await _context.Likes
            .Where(like => like.UserId == user.FirebaseId)
            .Select(like => like.ClimbingRouteId)
            .ToListAsync();

        List<long> bookmarkedRoutes = await _context.Likes
            .Where(like => like.UserId == user.FirebaseId)
            .Select(like => like.ClimbingRouteId)
            .ToListAsync();

        user.LikedRouteIds = likedRoutes;
        user.SentRouteIds = sentRoutes;
        user.BookmarkedRouteIds = bookmarkedRoutes;
    }

    [HttpPost("{userId}/PromoteToAdmin")]
    [RequireSuperuser]
    public async Task<ActionResult> PromoteToAdmin(string userId) {
        User? user = await _context.Users.FirstOrDefaultAsync(us => us.FirebaseId == userId);
        if (user == null) return NotFound();
        var claims = new Dictionary<string, object>
        {
            { ClaimTypes.Role, ClaimRole.Admin }
        };

        await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(userId, claims);
        return Ok();
    }

    [HttpPost("{userId}/RevokeAdmin")]
    [RequireSuperuser]
    public async Task<ActionResult> RevokeAdmin(string userId) {
        User? user = await _context.Users.FirstOrDefaultAsync(us => us.FirebaseId == userId);
        if (user == null) return NotFound();
        var claims = new Dictionary<string, object>
        {
            { ClaimTypes.Role, ClaimRole.User }
        };

        await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(userId, claims);
        return Ok();
    }
}