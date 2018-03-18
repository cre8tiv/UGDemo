using DemoProject.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DemoProject.Data
{
    public class DbSeed
    {
        public static void SeedTestData(DemoProjectContext context)
        {
            context.SaveChanges();
        }
    }
}