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
    }
}