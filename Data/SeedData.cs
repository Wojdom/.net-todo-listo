using Microsoft.EntityFrameworkCore;
using TodoList.Data;

namespace ToDoList.Models;

public static class SeedData 
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new TodoDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<TodoDbContext>>()))
        {
            var log = serviceProvider.GetRequiredService<ILogger<Program>>();

            if (context.Todo.Any())
            {
                log.LogInformation("The database has already been seeded.");
                return; 
            }

            log.LogInformation("Seeding the database.");

            context.Todo.AddRange(
                new Todo
                {
                    Description = "Buy milk",
                    IsDone = false,
                    DueDate = DateTime.Parse("2022-01-01")
                },

                new Todo
                {
                    Description = "Buy bread",
                    IsDone = false,
                    DueDate = DateTime.Parse("2022-01-01")
                },

                new Todo
                {
                    Description = "Buy cheese",
                    IsDone = false,
                    DueDate = DateTime.Parse("2022-01-01")
                }
            );
            context.SaveChanges();
        }
    }
}