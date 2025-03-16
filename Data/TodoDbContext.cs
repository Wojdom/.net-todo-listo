using Microsoft.EntityFrameworkCore;

namespace TodoList.Data
{
    public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
    {
        public DbSet<ToDoList.Models.Todo> Todo { get; set; } = default!;
    }
}
