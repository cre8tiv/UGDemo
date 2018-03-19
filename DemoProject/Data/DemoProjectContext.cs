using System.Linq;
using Microsoft.EntityFrameworkCore;
using DemoProject.Models;

namespace DemoProject.Data
{
    public class DemoProjectContext : DbContext
    {
        public DemoProjectContext(DbContextOptions<DemoProjectContext> options) : base(options)
        {
        }

        public DbSet<Site> Site { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Site>()
                .Property(b => b.Url)
                .IsRequired();
        }
    }
}