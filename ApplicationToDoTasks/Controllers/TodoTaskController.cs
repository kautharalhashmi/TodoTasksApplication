using ApplicationToDoTasks.Context;
using ApplicationToDoTasks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ApplicationToDoTasks.Controllers
{
    public class TodoTaskController : Controller
    {
        private readonly AppDbContext _context;

        public TodoTaskController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Seed categories if none exist
            if (!await _context.Categories.AnyAsync())
            {
                _context.Categories.AddRange(
                    new Category { Name = "Home" },
                    new Category { Name = "Work" },
                    new Category { Name = "Personal" },
                    new Category { Name = "Fitness" },
                    new Category { Name = "Shopping" },
                    new Category { Name = "Finance" },
                    new Category { Name = "Education" },
                    new Category { Name = "Health" },
                    new Category { Name = "Travel" },
                    new Category { Name = "Entertainment" }
                );
                await _context.SaveChangesAsync();
            }

            var tasks = await _context.TodoTasks.Include(t => t.Category).ToListAsync();
            var categories = await _context.Categories.ToListAsync();

            var vm = new TodoTaskViewModel
            {
                Tasks = tasks,
                Categories = new SelectList(categories, "Id", "Name"),
                Priorities = new SelectList(Enum.GetValues(typeof(Priority)))
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(string title, string description, int priority, int categoryId)
        {
            try
            {
                // Simple validation
                if (string.IsNullOrWhiteSpace(title))
                {
                    TempData["Error"] = "Title is required";
                    return RedirectToAction("Index");
                }

                if (categoryId <= 0)
                {
                    TempData["Error"] = "Please select a category";
                    return RedirectToAction("Index");
                }

                // Create new task directly
                var task = new TodoTask
                {
                    Title = title.Trim(),
                    Description = description?.Trim() ?? "",
                    Priority = (Priority)priority,
                    CategoryId = categoryId,
                    IsCompleted = false
                };

                _context.TodoTasks.Add(task);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    TempData["Success"] = "Task added successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to save task";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var task = await _context.TodoTasks.FindAsync(id);
            if (task == null) return NotFound();

            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", task.CategoryId);
            ViewBag.Priorities = new SelectList(Enum.GetValues(typeof(Priority)), task.Priority);

            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string title, string description, int priority, int categoryId, bool isCompleted)
        {
            try
            {
                var task = await _context.TodoTasks.FindAsync(id);
                if (task == null) return NotFound();

                task.Title = title?.Trim() ?? "";
                task.Description = description?.Trim() ?? "";
                task.Priority = (Priority)priority;
                task.CategoryId = categoryId;
                task.IsCompleted = isCompleted;

                await _context.SaveChangesAsync();
                TempData["Success"] = "Task updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error updating task: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var task = await _context.TodoTasks.FindAsync(id);
                if (task != null)
                {
                    _context.TodoTasks.Remove(task);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Task deleted successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting task: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleComplete(int id)
        {
            try
            {
                var task = await _context.TodoTasks.FindAsync(id);
                if (task != null)
                {
                    task.IsCompleted = !task.IsCompleted;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error updating task: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}