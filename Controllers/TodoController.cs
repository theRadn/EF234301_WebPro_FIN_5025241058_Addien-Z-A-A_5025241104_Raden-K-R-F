using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace TodoApp.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly TodoDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TodoController(
            TodoDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);

            var items = _context.Todos
                .Where(t => t.UserId == userId)
                .OrderBy(t => t.IsCompleted)
                .ThenBy(t => t.Deadline)
                .ToList();

            return View(items);
        }


        [HttpPost]
        public IActionResult Add(string title, DateTime? deadline)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                _context.Todos.Add(new TodoItem
                {
                    Title = title,
                    Deadline = deadline,
                    IsCompleted = false,
                    UserId = _userManager.GetUserId(User)
                });

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Toggle(int id)
        {
            var item = _context.Todos.Find(id);
            if (item != null)
            {
                if (item.UserId != _userManager.GetUserId(User))
                    return Forbid();
                item.IsCompleted = !item.IsCompleted;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var item = _context.Todos.Find(id);
            if (item != null)
            {
               if (item.UserId != _userManager.GetUserId(User))
                    return Forbid();

                _context.Todos.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var item = _context.Todos.Find(id);
            if (item == null)
                return NotFound();
            if (item.UserId != _userManager.GetUserId(User))
                return Forbid();
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(int id, string title, DateTime? deadline)
        {
            var item = _context.Todos.Find(id);
            if (item != null && !string.IsNullOrWhiteSpace(title))
            {
                if (item.UserId != _userManager.GetUserId(User))
                    return Forbid();
                item.Title = title;
                item.Deadline = deadline;
                _context.SaveChanges();
    
            }

            return RedirectToAction("Index");
        }
    }
}

