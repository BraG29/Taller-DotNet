using Authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace Authentication.DataAccess;

public class AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(e => e.Username)
            .IsUnique();
    }
}