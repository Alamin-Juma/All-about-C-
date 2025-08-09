using Microsoft.EntityFrameworkCore;

namespace WebAPIDemo.Properties.Models
{
    public class ShirtContext : DbContext
    {
        public DbSet<Shirt> Shirts { get; set; }

        public ShirtContext(DbContextOptions<ShirtContext> options) : base(options) { }
    }
}