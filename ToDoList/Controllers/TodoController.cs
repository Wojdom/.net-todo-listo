using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;
using TodoList.Data;

namespace ToDoList.Controllers
{
    public class TodoController(ILogger<TodoController> logger, TodoDbContext context) : Controller
    {
        private readonly ILogger<TodoController> _logger = logger;

        private readonly TodoDbContext _dbContext = context;

        // GET: Todo
        public async Task<IActionResult> Index(string searchString)
        {
            if (_dbContext.Todos == null)
            {
                return Problem("Entity set 'Todos' is null");
            }

            var tasks = from t in _dbContext.Todos
                        where t.TasksListId == null
                        select t;
            if (!string.IsNullOrEmpty(searchString))
            {
                tasks = tasks.Where(
                    s => s.Description.Contains(searchString));
            }

            return View(await tasks.ToListAsync());
        }

        // GET: Todo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            this._logger.LogInformation("Details action called");

            if (id == null)
            {
                return NotFound();
            }

            var todo = await _dbContext.Todos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // GET: Todo/Create
        public IActionResult Create(int? tasksListId)
        {
            var todo = new Todo();
            if (tasksListId.HasValue)
            {
                todo.TasksListId = tasksListId;
            }
            
            // Get all available TasksLists for the dropdown
            ViewBag.TasksLists = _dbContext.TasksLists
                .Select(tl => new SelectListItem
                {
                    Value = tl.Id.ToString(),
                    Text = tl.Name,
                    Selected = tasksListId.HasValue && tl.Id == tasksListId.Value
                })
                .ToList();
            
            // Add a "None" option
            ViewBag.TasksLists.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "-- None --",
                Selected = !tasksListId.HasValue
            });
            
            return View(todo);
        }

        // POST: Todo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,IsDone,DueDate,TasksListId")] Todo todo)
        {
            this._logger.LogDebug("Create action called");

            if (ModelState.IsValid)
            {
                this._logger.LogDebug("Model is valid");
                _dbContext.Add(todo);
                await _dbContext.SaveChangesAsync();
                
                // If the todo belongs to a TasksList, redirect back to that list's details
                if (todo.TasksListId.HasValue)
                {
                    return RedirectToAction("Details", "TasksLists", new { id = todo.TasksListId.Value });
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // GET: Todo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _dbContext.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            
            // Get all available TasksLists for the dropdown
            ViewBag.TasksLists = _dbContext.TasksLists
                .Select(tl => new SelectListItem
                {
                    Value = tl.Id.ToString(),
                    Text = tl.Name,
                    Selected = todo.TasksListId.HasValue && tl.Id == todo.TasksListId.Value
                })
                .ToList();
            
            // Add a "None" option
            ViewBag.TasksLists.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "-- None --",
                Selected = !todo.TasksListId.HasValue
            });
            
            return View(todo);
        }

        // POST: Todo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,IsDone,DueDate")] Todo todo)
        {
            if (id != todo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    todo.UpdatedAt = DateTime.Now;
                    _dbContext.Update(todo);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoExists(todo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // POST: Todo/MarkCompleted/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkCompleted(int id)
        {
            var todo = await _dbContext.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.IsDone = true;
            todo.UpdatedAt = DateTime.Now;
            
            _dbContext.Update(todo);
            await _dbContext.SaveChangesAsync();
            
            // If the todo belongs to a TasksList, redirect back to that list's details
            if (todo.TasksListId.HasValue)
            {
                return RedirectToAction("Details", "TasksLists", new { id = todo.TasksListId.Value });
            }
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Todo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _dbContext.Todos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // POST: Todo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todo = await _dbContext.Todos.FindAsync(id);
            if (todo != null)
            {
                _dbContext.Todos.Remove(todo);
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoExists(int id)
        {
            return _dbContext.Todos.Any(e => e.Id == id);
        }
    }
}
