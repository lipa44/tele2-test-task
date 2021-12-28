namespace DataAccess;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        => Database.EnsureCreatedAsync();

    public DbSet<Citizen> Citizens { get; set; }
}