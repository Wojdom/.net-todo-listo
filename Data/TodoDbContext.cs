using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace TodoList.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext (DbContextOptions<TodoDbContext> options)
            : base(options)
        {
        }

        public DbSet<ToDoList.Models.Todo> Todo { get; set; } = default!;
    }
}
