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

            if (context.Todos.Any())
            {
                log.LogInformation("The database has already been seeded.");
                return; 
            }

            log.LogInformation("Seeding the database.");

            context.Todos.AddRange(
                new Todo
                {
                    Description = "Buy pc",
                    IsDone = false,
                    DueDate = DateTime.Parse("2022-01-01")
                },

                new Todo
                {
                    Description = "Buy keyboard",
                    IsDone = false,
                    DueDate = DateTime.Parse("2022-01-01")
                },

                new Todo
                {
                    Description = "Remember about birthday",
                    IsDone = false,
                    DueDate = DateTime.Parse("2022-01-01")
                }
            );

            context.TasksLists.AddRange(
                new TasksList
                {
                    Name = "Grocery List",
                    Todos = new List<Todo>
                    {
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
                    }
                }
            );

            context.SaveChanges();
        }
    }
}