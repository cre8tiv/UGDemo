using DemoProject.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DemoProject.Data
{
    public class DbInit
    {
        public static void Initialize(DemoProjectContext context)
        {
            context.Database.Migrate();

            context.Database.EnsureCreated();

            context.SaveChanges();
        }
    }
}
