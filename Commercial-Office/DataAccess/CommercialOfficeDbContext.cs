using Commercial_Office.Model;
using Microsoft.EntityFrameworkCore;

namespace Commercial_Office.DataAccess
{
    public class CommercialOfficeDbContext(DbContextOptions<CommercialOfficeDbContext> options) : DbContext(options)
    {
        public DbSet<Office>? Offices { get; set; }
        public DbSet<AttentionPlace>? AttentionPlaces { get; set; }
    }
}
