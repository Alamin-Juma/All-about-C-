using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebAPIDemo.Properties.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Shirt> Shirts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}