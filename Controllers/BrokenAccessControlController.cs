using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OWASPTop10TaskManager.Data;
using System.Security.Claims;

namespace OWASPTaskManager.Controllers
{
    public class BrokenAccessControlController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BrokenAccessControlController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Vulnerable: No authorization checks
        public IActionResult EditTask(int id)
        {
            var task = _context.Tasks.Find(id);
            return View(task);
        }

        // Secure: Adding authorization checks
        [Authorize]
        public IActionResult EditTaskSecure(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var task = _context.Tasks.SingleOrDefault(t => t.Id == id && t.UserId == userId);
            if (task == null)
            {
                return Unauthorized();
            }
            return View(task);
        }


    }
}
