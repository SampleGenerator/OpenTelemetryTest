using Microsoft.EntityFrameworkCore;

namespace OpenTelemetrySample.Entities;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options)
        : base(options)
    {
        Posts = Set<Post>();
    }

    public DbSet<Post> Posts { get; init; }
}
