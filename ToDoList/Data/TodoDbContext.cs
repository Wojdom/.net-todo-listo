using Microsoft.EntityFrameworkCore;

namespace TodoList.Data
{
    public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
    {

        // TODO: potential refactor - todo model should be renamed to task
        public DbSet<ToDoList.Models.Todo> Todos { get; set; } = default!;

        public DbSet<ToDoList.Models.TasksList> TasksLists { get; set; } = default!;
    }
}
