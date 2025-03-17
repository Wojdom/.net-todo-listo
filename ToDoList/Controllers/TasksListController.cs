using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;
using TodoList.Data;


namespace ToDoList.Controllers
{
    public class TasksListsController(ILogger<TasksListsController> logger, TodoDbContext context) : Controller
    {
        private readonly ILogger<TasksListsController> _logger = logger;

        private readonly TodoDbContext _dbContext = context;

        // GET: TasksLists
        public async Task<IActionResult> Index()
        {
            this._logger.LogDebug("Tasks list action called");

            return View(await _dbContext.TasksLists.ToListAsync());
        }

        // GET: TasksLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            this._logger.LogDebug("Details action called");

            if (id == null)
            {
                return NotFound();
            }

            var todoList = await _dbContext.TasksLists
                        .Include(t => t.Todos) 
                        .FirstOrDefaultAsync(m => m.Id == id);
            if (todoList == null)
            {
                return NotFound();
            }

            return View(todoList);
        }

        // GET: TodoList/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TodoList/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] TasksList todoList)
        {
            this._logger.LogDebug("Create action called");

            if (ModelState.IsValid)
            {
                this._logger.LogDebug("Model is valid");
                _dbContext.Add(todoList);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todoList);
        }
    }
}