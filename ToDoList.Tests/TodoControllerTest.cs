using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TodoList.Data;
using ToDoList.Controllers;
using ToDoList.Models;
using Xunit;

namespace ToDoList.Tests
{
    public class TodoControllerTest : IDisposable
    {
        private readonly Mock<ILogger<TodoController>> _loggerMock;
        private readonly TodoDbContext _context;
        private readonly TodoController _controller;

        public TodoControllerTest()
        {
            _loggerMock = new Mock<ILogger<TodoController>>();

            // Set up in-memory database
            var options = new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(databaseName: $"TodoDb_{Guid.NewGuid()}")
                .Options;

            _context = new TodoDbContext(options);

            // Seed test data
            _context.Todos.AddRange(new List<Todo>
            {
                new Todo { Id = 1, Description = "Test task 1", IsDone = false, DueDate = DateTime.Now.AddDays(1) },
                new Todo { Id = 2, Description = "Test task 2", IsDone = true, DueDate = DateTime.Now.AddDays(2) },
                new Todo { Id = 3, Description = "Another task", IsDone = false, DueDate = DateTime.Now.AddDays(3) }
            });
            _context.SaveChanges();

            // Create the controller with the test dependencies
            _controller = new TodoController(_loggerMock.Object, _context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task Index_ReturnsViewWithAllTodos_WhenNoSearchString()
        {
            // Act
            var result = await _controller.Index(searchString: string.Empty);
            
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Todo>>(viewResult.Model);
            Assert.Equal(3, model.Count);
        }

        [Fact]
        public async Task Index_ReturnsFilteredTodos_WhenSearchStringProvided()
        {
            // Act
            var result = await _controller.Index(searchString: "Another");
            
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Todo>>(viewResult.Model);
            Assert.Single(model);
            Assert.Equal("Another task", model.First().Description);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            // Act
            var result = await _controller.Details(id: null);
            
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenTodoDoesNotExist()
        {
            // Act
            var result = await _controller.Details(id: 99);
            
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public async Task Details_ReturnsViewWithTodo_WhenTodoExists()
        {
            // Act
            var result = await _controller.Details(id: 1);
            
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Todo>(viewResult.Model);
            Assert.Equal(1, model.Id);
            Assert.Equal("Test task 1", model.Description);
        }
    }
}