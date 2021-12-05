using System.Data.Entity.Infrastructure;

using Backend.Models;

using Microsoft.EntityFrameworkCore;

namespace Backend.Context;

public class ClimbingRouteContext : DbContext {
    public ClimbingRouteContext(DbContextOptions<ClimbingRouteContext> options)
        : base(options) {}

    public DbSet<ClimbingRoute> Routes => Set<ClimbingRoute>();
}