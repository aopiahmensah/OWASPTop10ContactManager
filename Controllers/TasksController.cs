using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using OWASPTaskManager.Models;
using OWASPTop10TaskManager.Data;

namespace OWASPTaskManager.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> IndexVulnerable()
        {
            var tasks = await _context.Tasks.ToListAsync();
            return View(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVulnerable(Tasks task)
        {
            if (ModelState.IsValid)
            {
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        [HttpGet]
        public async Task<IActionResult> IndexSecure()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tasks = await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();
            return View(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSecure(Tasks task)
        {
            if (ModelState.IsValid)
            {
                task.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

    }
}
