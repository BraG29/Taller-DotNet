using Microsoft.EntityFrameworkCore;
using Quality_Management.Model;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Quality_Management.DataAccess;

public class QualityManagementDbContext(DbContextOptions<QualityManagementDbContext> options) : DbContext(options)
{
    public DbSet<Procedure> Procedures { get; set; }
    public DbSet<Office> Offices { get; set;  }
}