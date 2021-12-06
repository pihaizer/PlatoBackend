using Backend.Context;
using Backend.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers; 

[ApiController]
[Route("api/[controller]")]
public class LikeController : ControllerBase {
    readonly PlatoContext _context;

    public LikeController(PlatoContext context) {
        _context = context;
    }
}