using System.Data.Entity.Infrastructure;

using Backend.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Context;

public class PlatoContext : DbContext {
    public PlatoContext(DbContextOptions<PlatoContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<ClimbingRoute> Routes => Set<ClimbingRoute>();
    public DbSet<ClimbingRouteTag> RouteTags => Set<ClimbingRouteTag>();
    public DbSet<Like> Likes => Set<Like>();
    public DbSet<Send> Sends => Set<Send>();
    public DbSet<ClimbingRouteBookmark> Bookmarks => Set<ClimbingRouteBookmark>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<ClimbingRouteModel> RouteModels => Set<ClimbingRouteModel>();
    public DbSet<News> News => Set<News>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ClimbingRouteTag>()
            .HasKey(nameof(ClimbingRouteTag.TagId), nameof(ClimbingRouteTag.ClimbingRouteId));
    }
}