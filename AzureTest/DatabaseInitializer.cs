using AzureTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureTest
{
    public static class DbInitializer
    {
        public static void Initialize(DatabaseContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Todo.Any())
            {
                return;   // DB has been seeded
            }
            

            var todos = new Todo[]
            {
                new Todo{Description="D1", CreatedDate = DateTime.UtcNow },
                new Todo{Description="D2", CreatedDate = DateTime.UtcNow.AddDays(1) },
                new Todo{Description="D3", CreatedDate = DateTime.UtcNow.AddDays(2) },
            };
            foreach (var e in todos)
            {
                context.Todo.Add(e);
            }
            context.SaveChanges();
        }
    }
}
