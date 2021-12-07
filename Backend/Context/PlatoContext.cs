using System.Data.Entity.Infrastructure;

using Backend.Models;

using Microsoft.EntityFrameworkCore;

namespace Backend.Context;

public class PlatoContext : DbContext {
    public PlatoContext(DbContextOptions<PlatoContext> options)
        : base(options) {}

    public DbSet<ClimbingRoute> Routes => Set<ClimbingRoute>();
    public DbSet<Like> Likes => Set<Like>();
    public DbSet<Send> Sends => Set<Send>();
    public DbSet<ClimbingRouteBookmark> Bookmarks => Set<ClimbingRouteBookmark>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<ClimbingRouteModel> RouteModels => Set<ClimbingRouteModel>();
}