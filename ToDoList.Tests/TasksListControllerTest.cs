using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Data;
using ToDoList.Controllers;
using ToDoList.Models;
using Xunit;

namespace ToDoList.Tests
{
    public class TasksListsControllerTests : IDisposable
    {
        private readonly TodoDbContext _context;
        private readonly ILogger<TasksListsController> _logger;
        private readonly TasksListsController _controller;

        public TasksListsControllerTests()
        {
            var options = new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(databaseName: $"TasksListsDb_{Guid.NewGuid()}")
                .Options;

            _context = new TodoDbContext(options);
            
            SeedDatabase();

            _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<TasksListsController>();
            _controller = new TasksListsController(_logger, _context);
        }

        private void SeedDatabase()
        {
            var tasksList1 = new TasksList { Id = 1, Name = "Work Tasks", CreatedAt = DateTime.Now.AddDays(-10), UpdatedAt = DateTime.Now.AddDays(-5) };
            var tasksList2 = new TasksList { Id = 2, Name = "Home Tasks", CreatedAt = DateTime.Now.AddDays(-7), UpdatedAt = DateTime.Now.AddDays(-2) };
            
            _context.TasksLists.AddRange(tasksList1, tasksList2);
            
            _context.Todos.AddRange(
                new Todo { Id = 1, Description = "Task 1", IsDone = false, TasksListId = 1, DueDate = DateTime.Now.AddDays(1) },
                new Todo { Id = 2, Description = "Task 2", IsDone = true, TasksListId = 1, DueDate = DateTime.Now.AddDays(2) },
                new Todo { Id = 3, Description = "Task 3", IsDone = false, TasksListId = 2, DueDate = DateTime.Now.AddDays(3) }
            );
            
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
        
        [Fact]
        public async Task Index_ReturnsViewWithAllTasksLists()
        {
            var result = await _controller.Index();
            
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<TasksList>>(viewResult.Model);
            Assert.Equal(2, model.Count());
        }
        
        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            var result = await _controller.Details(null);
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public async Task Details_ReturnsNotFound_WhenTasksListDoesNotExist()
        {
            var result = await _controller.Details(99);
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public async Task Details_ReturnsViewWithTasksList_WhenTasksListExists()
        {
            var result = await _controller.Details(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<TasksList>(viewResult.Model);
            Assert.Equal(1, model.Id);
            Assert.Equal("Work Tasks", model.Name);

            Assert.Equal(2, model.Todos.Count);
            Assert.Contains(model.Todos, t => t.Description == "Task 1");
            Assert.Contains(model.Todos, t => t.Description == "Task 2");
        }
        
        [Fact]
        public void Create_GET_ReturnsView()
        {
            var result = _controller.Create();

            Assert.IsType<ViewResult>(result);
        }
        
        [Fact]
        public async Task Create_POST_RedirectsToIndex_WhenModelIsValid()
        {
            var newTasksList = new TasksList { Name = "New Task List" };
            
            var result = await _controller.Create(newTasksList);
            
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(TasksListsController.Index), redirectResult.ActionName);
            
            var tasksListInDb = await _context.TasksLists.FirstOrDefaultAsync(t => t.Name == "New Task List");
            Assert.NotNull(tasksListInDb);
   
            Assert.NotEqual(default, tasksListInDb.CreatedAt);
            Assert.NotEqual(default, tasksListInDb.UpdatedAt);
        }
        
        [Fact]
        public async Task Create_POST_ReturnsView_WhenModelStateIsInvalid()
        {
            var invalidTasksList = new TasksList { Name = null }; // Name is required
            _controller.ModelState.AddModelError("Name", "Required");
            
            var result = await _controller.Create(invalidTasksList);
            
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<TasksList>(viewResult.Model);
            Assert.Equal(invalidTasksList, model);
        }
        
        [Fact]
        public async Task Index_LoadsTasksLists_WithTodos()
        {
            // Act
            var result = await _controller.Index();
            
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<TasksList>>(viewResult.Model);
            
            // Check if todos are loaded
            var firstList = model.First(t => t.Id == 1);
            Assert.NotEmpty(firstList.Todos); 
        }
    }
}