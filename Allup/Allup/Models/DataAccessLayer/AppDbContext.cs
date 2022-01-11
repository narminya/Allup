using Allup.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Allup.Models.DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }
        public DbSet<Category> categories { get; set; }

    }
}

